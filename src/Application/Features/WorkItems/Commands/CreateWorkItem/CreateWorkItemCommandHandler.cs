using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Domain.Entities;

namespace Netdemo.Application.Features.WorkItems.Commands.CreateWorkItem;

public sealed class CreateWorkItemCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IAuditLogService auditLogService)
    : IRequestHandler<CreateWorkItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

        if (project is null)
        {
            throw new NotFoundException("Project not found.");
        }

        if (project.OrganizationId != currentUserService.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        var workItem = new WorkItem(request.ProjectId, request.Title, request.Description);
        dbContext.WorkItems.Add(workItem);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogService.WriteAsync(currentUserService.OrganizationId, "WorkItemCreated", $"Work item '{workItem.Title}' created in project {request.ProjectId}", cancellationToken);
        return workItem.Id;
    }
}
