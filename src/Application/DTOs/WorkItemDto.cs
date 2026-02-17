namespace Netdemo.Application.DTOs;

public sealed record WorkItemDto(Guid Id, Guid ProjectId, string Title, string? Description, bool IsCompleted, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt);
