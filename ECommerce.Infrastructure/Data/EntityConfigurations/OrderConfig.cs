using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.EntityConfigurations;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShipToAddress, a =>
        {
            a.WithOwner();
            a.Property(address => address.FirstName).IsRequired().HasMaxLength(100);
            a.Property(address => address.LastName).IsRequired().HasMaxLength(100);
            a.Property(address => address.Street).IsRequired().HasMaxLength(200);
            a.Property(address => address.City).IsRequired().HasMaxLength(100);
            a.Property(address => address.State).IsRequired().HasMaxLength(100);
            a.Property(address => address.ZipCode).IsRequired().HasMaxLength(20);
        });

        builder.Navigation(o => o.ShipToAddress).IsRequired();

        builder.Property(s => s.Status)
            .HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.Subtotal)
            .HasPrecision(10, 2);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
