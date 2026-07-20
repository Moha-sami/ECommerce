using ECommerce.Application.common;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.Application.Contacts;

public interface IOrderService
{
    Task<Result<OrderDto>> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, OrderAddressDto shippingAddress);
    Task<Result<IReadOnlyList<OrderDto>>> GetOrdersForUserAsync(string buyerEmail);
    Task<Result<OrderDto>> GetOrderByIdForUserAsync(int id, string buyerEmail);
    Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync();
    Task<Result<OrderDto>> UpdateOrderPaymentStatusAsync(string paymentIntentId, OrderStatus status);
}
