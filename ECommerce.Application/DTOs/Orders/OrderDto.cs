namespace ECommerce.Application.DTOs.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; } = string.Empty;
    public DateTimeOffset OrderDate { get; set; }
    public OrderAddressDto ShipToAddress { get; set; } = null!;
    public string DeliveryMethod { get; set; } = string.Empty;
    public decimal ShippingPrice { get; set; }
    public IReadOnlyList<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}
