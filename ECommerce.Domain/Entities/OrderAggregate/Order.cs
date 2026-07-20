namespace ECommerce.Domain.Entities.OrderAggregate;

public class Order : BaseEntity<int>
{
    public Order()
    {
    }

    public Order(
        string buyerEmail,
        OrderAddress shipToAddress,
        DeliveryMethod deliveryMethod,
        IReadOnlyList<OrderItem> orderItems,
        decimal subtotal,
        string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShipToAddress = shipToAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        Subtotal = subtotal;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; } = string.Empty;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    public OrderAddress ShipToAddress { get; set; } = null!;
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
    public int DeliveryMethodId { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; } = string.Empty;

    public decimal GetTotal()
    {
        return Subtotal + (DeliveryMethod != null ? DeliveryMethod.Price : 0);
    }
}
