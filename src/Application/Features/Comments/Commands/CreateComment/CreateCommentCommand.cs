using MediatR;
using Netdemo.Application.DTOs;

namespace Netdemo.Application.Features.Comments.Commands.CreateComment;

public sealed record CreateCommentCommand(Guid WorkItemId, string Content) : IRequest<CommentDto>;
