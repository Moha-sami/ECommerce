namespace ECommerce.Domain.DataSeeding
{
    public interface IDataSeeder
    {
        Task SeedDataAsync(CancellationToken ct=default);
    }
}
