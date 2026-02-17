using MediatR;
using Netdemo.Application.DTOs;

namespace Netdemo.Application.Features.WorkItems.Queries.GetByProject;

public sealed record GetWorkItemsByProjectQuery(Guid ProjectId) : IRequest<IReadOnlyCollection<WorkItemDto>>;
