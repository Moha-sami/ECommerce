namespace ECommerce.Infrastructure.Data;

using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductBrand> Brands { get; set; } = null!;
    public DbSet<ProductType> ProductTypes { get; set; } = null!;

}
