using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Infrastructure.Identity;

namespace Netdemo.Infrastructure.Services;

public sealed class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<IdentityUserProfile?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (user is null)
        {
            return null;
        }

        var passwordOk = await userManager.CheckPasswordAsync(user, password);
        if (!passwordOk)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);
        return new IdentityUserProfile(user.Id, user.Email ?? normalizedEmail, user.OrganizationId, roles);
    }

    public async Task<RegistrationResult> RegisterAsync(string email, string password, Guid organizationId, IEnumerable<string> roles, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            OrganizationId = organizationId,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return new RegistrationResult(false, null, createResult.Errors.Select(e => e.Description).ToArray());
        }

        var rolesResult = await userManager.AddToRolesAsync(user, roles);
        if (!rolesResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return new RegistrationResult(false, null, rolesResult.Errors.Select(e => e.Description).ToArray());
        }

        return new RegistrationResult(true, user.Id, []);
    }
}
