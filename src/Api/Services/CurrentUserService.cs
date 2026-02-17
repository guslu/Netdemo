using System.Security.Claims;
using Netdemo.Application.Abstractions;

namespace Netdemo.Api.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId => ParseGuidClaim(ClaimTypes.NameIdentifier);

    public Guid OrganizationId => ParseGuidClaim("organization_id");

    public bool IsInRole(string role)
    {
        var user = httpContextAccessor.HttpContext?.User;
        return user?.IsInRole(role) ?? false;
    }

    private Guid ParseGuidClaim(string claimType)
    {
        var value = httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
        if (!Guid.TryParse(value, out var parsed))
        {
            return Guid.Empty;
        }

        return parsed;
    }
}
