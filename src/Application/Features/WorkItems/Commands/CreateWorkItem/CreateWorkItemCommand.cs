using MediatR;

namespace Netdemo.Application.Features.WorkItems.Commands.CreateWorkItem;

public sealed record CreateWorkItemCommand(Guid ProjectId, string Title, string? Description) : IRequest<Guid>;
