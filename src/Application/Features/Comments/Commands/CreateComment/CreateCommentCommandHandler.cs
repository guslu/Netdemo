using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Application.DTOs;
using Netdemo.Domain.Entities;

namespace Netdemo.Application.Features.Comments.Commands.CreateComment;

public sealed class CreateCommentCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IAuditLogService auditLogService)
    : IRequestHandler<CreateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var workItem = await dbContext.WorkItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.WorkItemId, cancellationToken)
            ?? throw new NotFoundException("Work item not found.");

        var project = await dbContext.Projects.AsNoTracking().FirstAsync(x => x.Id == workItem.ProjectId, cancellationToken);
        if (project.OrganizationId != currentUserService.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        var comment = new Comment(request.WorkItemId, currentUserService.UserId, request.Content);
        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogService.WriteAsync(currentUserService.OrganizationId, "CommentCreated", $"Comment added to work item {request.WorkItemId}", cancellationToken);

        return new CommentDto(comment.Id, comment.WorkItemId, comment.AuthorId, comment.Content, comment.CreatedAt);
    }
}
