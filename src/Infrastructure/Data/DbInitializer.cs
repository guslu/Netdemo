using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netdemo.Domain.Entities;
using Netdemo.Infrastructure.Identity;

namespace Netdemo.Infrastructure.Data;

public static class DbInitializer
{
    private static readonly string[] Roles = ["Admin", "Manager", "Member"];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var scopedServices = scope.ServiceProvider;

        var context = scopedServices.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        await SeedBootstrapAdminAsync(scopedServices, context);
    }

    private static async Task SeedBootstrapAdminAsync(IServiceProvider serviceProvider, ApplicationDbContext context)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var adminEmail = configuration["BootstrapAdmin:Email"];
        var adminPassword = configuration["BootstrapAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var existing = await userManager.FindByEmailAsync(adminEmail);
        if (existing is not null)
        {
            return;
        }

        var organization = await context.Organizations.FirstOrDefaultAsync();
        if (organization is null)
        {
            organization = new Organization("Default Organization");
            context.Organizations.Add(organization);
            await context.SaveChangesAsync();
        }

        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            OrganizationId = organization.Id
        };

        var createResult = await userManager.CreateAsync(user, adminPassword);
        if (!createResult.Succeeded)
        {
            return;
        }

        await userManager.AddToRoleAsync(user, "Admin");
    }
}
