namespace Netdemo.Application.DTOs;

public sealed record CommentDto(Guid Id, Guid WorkItemId, Guid AuthorId, string Content, DateTimeOffset CreatedAt);
