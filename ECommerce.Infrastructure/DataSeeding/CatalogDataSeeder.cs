using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Infrastructure.DataSeeding;

public class CatalogDataSeeder(AppDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
{
    public async Task SeedDataAsync(CancellationToken ct = default)
    {
        try
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync(ct);

            var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");
            await SeedIFEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
            await SeedIFEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);
            await SeedIFEmptyAsync<Product, int>(seedRoot, "products.json", ct);

            if (!await dbContext.DeliveryMethods.AnyAsync(ct))
            {
                var deliveryMethods = new List<DeliveryMethod>
                {
                    new() { ShortName = "UPS Ground",        DeliveryTime = "3-5 Days",  Description = "Fastest delivery to home or office",  Price = 10m },
                    new() { ShortName = "UPS Express",       DeliveryTime = "1-2 Days",  Description = "Get your order in 1-2 days",          Price = 25m },
                    new() { ShortName = "Standard Shipping", DeliveryTime = "5-7 Days",  Description = "Slower but cheap option",              Price = 5m  },
                    new() { ShortName = "Free Shipping",     DeliveryTime = "7-10 Days", Description = "Free shipping for all orders",         Price = 0m  }
                };
                await dbContext.DeliveryMethods.AddRangeAsync(deliveryMethods, ct);
            }

            int result = await dbContext.SaveChangesAsync(ct);
            if (result > 0)
                logger.LogInformation("{Count} Rows Seeded Successfully", result);
            else
                logger.LogInformation("Data Already Seeded");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while seeding catalog data.", ex);
        }
    }

    private async Task SeedIFEmptyAsync<T, TKey>(string rootPath, string fileName, CancellationToken ct) where T : BaseEntity<TKey>
    {
        if (await dbContext.Set<T>().AnyAsync(ct))
        {
            logger.LogInformation("Tables Already Have Data");
            return;
        }

        var filePath = Path.Combine(rootPath, fileName);
        if (!File.Exists(filePath))
        {
            logger.LogInformation("File {FileName} does not exist", fileName);
            return;
        }

        using var fileStream = File.OpenRead(filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, options, ct);

        if (items?.Any() ?? false)
            dbContext.Set<T>().AddRange(items);
    }
}
