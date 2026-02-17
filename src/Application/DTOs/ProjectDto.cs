namespace Netdemo.Application.DTOs;

public sealed record ProjectDto(Guid Id, string Name, string? Description, DateTimeOffset CreatedAt);
