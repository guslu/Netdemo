using MediatR;

namespace Netdemo.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResult>;

public sealed record LoginResult(string AccessToken, DateTimeOffset ExpiresAtUtc, Guid OrganizationId, IReadOnlyCollection<string> Roles);
