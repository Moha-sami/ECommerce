using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.DataSeeding;

public class IdentityDataSeeder(
    AppIdentityDbContext dbContext,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ILogger<IdentityDataSeeder> logger) : IDataSeeder
{
    private const string AdminRole = "Admin";
    private const string AdminEmail = "admin@ecommerce.com";

    public async Task SeedDataAsync(CancellationToken ct = default)
    {
        try
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync(ct);

            // Seed Admin role
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
                logger.LogInformation("Admin role created.");
            }

            // Seed Admin user
            if (await userManager.FindByEmailAsync(AdminEmail) == null)
            {
                var password = $"Admin@{Guid.NewGuid().ToString("N")[..8]}!";
                var admin = new AppUser
                {
                    DisplayName = "Admin",
                    Email = AdminEmail,
                    UserName = AdminEmail
                };

                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AdminRole);
                    // Print to console — this is the only time this password is visible
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("===========================================");
                    Console.WriteLine($"  Admin user created on first run");
                    Console.WriteLine($"  Email:    {AdminEmail}");
                    Console.WriteLine($"  Password: {password}");
                    Console.WriteLine("===========================================");
                    Console.ResetColor();
                    logger.LogInformation("Admin user seeded.");
                }
                else
                {
                    foreach (var error in result.Errors)
                        logger.LogError("Admin seed error: {Code} - {Description}", error.Code, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while seeding identity data.", ex);
        }
    }
}
