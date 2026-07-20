using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.EntityConfigurations;

public class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(d => d.Price)
            .HasPrecision(10, 2);

        builder.Property(d => d.ShortName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.DeliveryTime).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).IsRequired().HasMaxLength(500);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
