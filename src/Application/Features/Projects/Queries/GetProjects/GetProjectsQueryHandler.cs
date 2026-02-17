using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.DTOs;

namespace Netdemo.Application.Features.Projects.Queries.GetProjects;

public sealed class GetProjectsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetProjectsQuery, IReadOnlyCollection<ProjectDto>>
{
    public async Task<IReadOnlyCollection<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Projects
            .AsNoTracking()
            .Where(p => p.OrganizationId == request.OrganizationId)
            .Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
