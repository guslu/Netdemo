using Microsoft.AspNetCore.Identity;

namespace Netdemo.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public Guid OrganizationId { get; set; }
}
