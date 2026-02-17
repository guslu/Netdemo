using MediatR;

namespace Netdemo.Application.Features.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(Guid OrganizationId, string Name, string? Description) : IRequest<Guid>;
