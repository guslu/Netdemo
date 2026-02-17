using MediatR;

namespace Netdemo.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string Email, string Password, Guid OrganizationId, IReadOnlyCollection<string> Roles) : IRequest<RegisterResult>;

public sealed record RegisterResult(Guid UserId);
