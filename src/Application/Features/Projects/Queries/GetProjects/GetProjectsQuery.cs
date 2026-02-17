using MediatR;
using Netdemo.Application.DTOs;

namespace Netdemo.Application.Features.Projects.Queries.GetProjects;

public sealed record GetProjectsQuery(Guid OrganizationId) : IRequest<IReadOnlyCollection<ProjectDto>>;
