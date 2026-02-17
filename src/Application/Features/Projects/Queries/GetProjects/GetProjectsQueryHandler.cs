using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Application.DTOs;

namespace Netdemo.Application.Features.Projects.Queries.GetProjects;

public sealed class GetProjectsQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<GetProjectsQuery, IReadOnlyCollection<ProjectDto>>
{
    public async Task<IReadOnlyCollection<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        if (request.OrganizationId != currentUserService.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        var exists = await dbContext.Organizations.AsNoTracking().AnyAsync(x => x.Id == request.OrganizationId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException("Organization not found.");
        }

        return await dbContext.Projects
            .AsNoTracking()
            .Where(x => x.OrganizationId == request.OrganizationId)
            .OrderBy(x => x.Name)
            .Select(x => new ProjectDto(x.Id, x.OrganizationId, x.Name, x.Description, x.CreatedAt, x.UpdatedAt))
            .ToListAsync(cancellationToken);
    }
}
