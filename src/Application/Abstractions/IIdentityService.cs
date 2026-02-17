namespace Netdemo.Application.Abstractions;

public interface IIdentityService
{
    Task<AuthenticatedUser?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
}

public sealed record AuthenticatedUser(Guid UserId, string Email, IReadOnlyCollection<string> Roles);
