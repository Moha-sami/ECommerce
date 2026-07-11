using ECommerce.Domain.DataSeeding;
using ECommerce.Infrastructure.DataSeeding;

namespace ECommerce.API.Extension
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dataSeeder = services.GetRequiredKeyedService<IDataSeeder>(nameof(CatalogDataSeeder));
                await dataSeeder.SeedDataAsync();
                return app;
            }
        }
    }
}
