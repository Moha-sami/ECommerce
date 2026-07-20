using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.Orders;

public class OrderCreateDto
{
    [Required]
    public string BasketId { get; set; } = string.Empty;

    [Required]
    public int DeliveryMethodId { get; set; }

    [Required]
    public OrderAddressDto ShipToAddress { get; set; } = null!;
}
