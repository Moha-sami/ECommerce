using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.EntityConfigurations;

public class BrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        
        builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
