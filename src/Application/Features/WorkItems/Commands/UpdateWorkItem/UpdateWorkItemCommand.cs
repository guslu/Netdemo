using MediatR;

namespace Netdemo.Application.Features.WorkItems.Commands.UpdateWorkItem;

public sealed record UpdateWorkItemCommand(Guid Id, string Title, string? Description, bool IsCompleted) : IRequest;
