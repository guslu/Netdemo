using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Application.DTOs;

namespace Netdemo.Application.Features.WorkItems.Queries.GetByProject;

public sealed class GetWorkItemsByProjectQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<GetWorkItemsByProjectQuery, IReadOnlyCollection<WorkItemDto>>
{
    public async Task<IReadOnlyCollection<WorkItemDto>> Handle(GetWorkItemsByProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Project not found.");

        if (project.OrganizationId != currentUserService.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        return await dbContext.WorkItems.AsNoTracking()
            .Where(x => x.ProjectId == request.ProjectId)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new WorkItemDto(x.Id, x.ProjectId, x.Title, x.Description, x.IsCompleted, x.CreatedAt, x.UpdatedAt))
            .ToListAsync(cancellationToken);
    }
}
