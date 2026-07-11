using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Infrastructure.DataSeeding;

public class CatalogDataSeeder(AppDbContext dbContext,ILogger<CatalogDataSeeder> logger) : IDataSeeder
{
    public async Task SeedDataAsync(CancellationToken ct = default)
    {
        try
        {
            //Check if we have previous migrations that have not been applied yet
            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
            if (PendingMigrations.Any()) 
           await dbContext.Database.MigrateAsync(ct);
            //seeding the catalog data
            //path to the JSON file containing the catalog data
            var SeedRoot= Path.Combine(AppContext.BaseDirectory, "DataSeed");
           await SeedIFEmptyAsync<ProductBrand, int>(SeedRoot, "brands.json", ct);
           await SeedIFEmptyAsync<ProductType, int>(SeedRoot, "types.json", ct);
           await SeedIFEmptyAsync<Product, int>(SeedRoot, "products.json", ct);
         int Result=  await dbContext.SaveChangesAsync(ct);
            if (Result > 0)
                logger.LogInformation($"{Result} Rows Seeded Successfully");
            else
                logger.LogInformation($" Data Already Seeded");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new Exception("An error occurred while seeding catalog data.", ex);
        }
        //
    }
    private async Task SeedIFEmptyAsync<T, Tkey>(string rootpath, string FileName, CancellationToken ct) where T : BaseEntity<Tkey>
    {
        if (await dbContext.Set<T>().AnyAsync(ct))
        {
            logger.LogInformation($"Tables Already Have Data");
            return;
        }

        var filePath = Path.Combine(rootpath, FileName);
        if (!File.Exists(filePath))
        {
            logger.LogInformation($"File {FileName} Is not Exists");
            return;
        }

        using var FileStream = File.OpenRead(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var items = await JsonSerializer.DeserializeAsync<List<T>>(FileStream, options, ct);

        if (items?.Any() ?? false)
            dbContext.Set<T>().AddRange(items);
    }
    
}
