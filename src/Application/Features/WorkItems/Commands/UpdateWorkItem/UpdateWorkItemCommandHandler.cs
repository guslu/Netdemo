using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;

namespace Netdemo.Application.Features.WorkItems.Commands.UpdateWorkItem;

public sealed class UpdateWorkItemCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IAuditLogService auditLogService)
    : IRequestHandler<UpdateWorkItemCommand>
{
    public async Task Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
    {
        var workItem = await dbContext.WorkItems.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Work item not found.");

        var project = await dbContext.Projects.AsNoTracking()
            .FirstAsync(x => x.Id == workItem.ProjectId, cancellationToken);

        if (project.OrganizationId != currentUserService.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        workItem.Update(request.Title, request.Description, request.IsCompleted);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditLogService.WriteAsync(currentUserService.OrganizationId, "WorkItemUpdated", $"Work item '{workItem.Title}' updated", cancellationToken);
    }
}
