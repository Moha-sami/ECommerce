namespace ECommerce.Domain.Entities.OrderAggregate;

public class OrderItem : BaseEntity<int>
{
    public OrderItem()
    {
    }

    public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
    {
        ItemOrdered = itemOrdered;
        Price = price;
        Quantity = quantity;
    }

    public ProductItemOrdered ItemOrdered { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
