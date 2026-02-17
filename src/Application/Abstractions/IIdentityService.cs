namespace Netdemo.Application.Abstractions;

public interface IIdentityService
{
    Task<IdentityUserProfile?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<RegistrationResult> RegisterAsync(string email, string password, Guid organizationId, IEnumerable<string> roles, CancellationToken cancellationToken = default);
}

public sealed record IdentityUserProfile(Guid UserId, string Email, Guid OrganizationId, IReadOnlyCollection<string> Roles);

public sealed record RegistrationResult(bool Succeeded, Guid? UserId, IReadOnlyCollection<string> Errors);
