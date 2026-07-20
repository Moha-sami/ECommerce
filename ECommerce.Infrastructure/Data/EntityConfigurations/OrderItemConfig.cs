using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.EntityConfigurations;

public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(i => i.ItemOrdered, io =>
        {
            io.WithOwner();
            io.Property(p => p.ProductName).IsRequired().HasMaxLength(100);
            io.Property(p => p.PictureUrl).IsRequired();
        });

        builder.Navigation(i => i.ItemOrdered).IsRequired();

        builder.Property(i => i.Price)
            .HasPrecision(10, 2);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
