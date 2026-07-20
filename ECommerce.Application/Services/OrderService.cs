using AutoMapper;
using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> CreateOrderAsync(
        string buyerEmail,
        string basketId,
        int deliveryMethodId,
        OrderAddressDto shippingAddress)
    {
        // 1. Get basket from basket repository
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null)
        {
            return Result<OrderDto>.Fail(Error.NotFound("Basket.NotFound", "Customer basket was not found"));
        }

        // 2. Get items from product repo & recalculate prices from DB
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
            if (productItem == null)
            {
                return Result<OrderDto>.Fail(Error.NotFound("Product.NotFound", $"Product with ID {item.Id} was not found"));
            }

            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // 3. Get delivery method from repository
        var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(deliveryMethodId);
        if (deliveryMethod == null)
        {
            return Result<OrderDto>.Fail(Error.NotFound("DeliveryMethod.NotFound", "Selected delivery method was not found"));
        }

        // 4. Calculate subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);

        // 5. Map shipping address
        var shippingAddressEntity = _mapper.Map<OrderAddressDto, OrderAddress>(shippingAddress);

        // 6. Create order
        var order = new Order(buyerEmail, shippingAddressEntity, deliveryMethod, items, subtotal, basket.PaymentIntentId ?? string.Empty);

        // 7. Save order to DB
        _unitOfWork.GetRepository<Order, int>().Add(order);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
        {
            return Result<OrderDto>.Fail(Error.InternalServerError("Order.CreateFailed", "Problem creating order"));
        }

        // 8. Map to OrderDto and return
        var orderDto = _mapper.Map<Order, OrderDto>(order);
        return Result<OrderDto>.Ok(orderDto);
    }

    public async Task<Result<IReadOnlyList<OrderDto>>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        var orders = await _unitOfWork.GetRepository<Order, int>().ListWithSpecAsync(spec);

        var orderDtos = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderDto>>(orders);
        return Result<IReadOnlyList<OrderDto>>.Ok(orderDtos);
    }

    public async Task<Result<OrderDto>> GetOrderByIdForUserAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        var order = await _unitOfWork.GetRepository<Order, int>().GetEntityWithSpec(spec);

        if (order == null)
        {
            return Result<OrderDto>.Fail(Error.NotFound("Order.NotFound", "Order not found"));
        }

        var orderDto = _mapper.Map<Order, OrderDto>(order);
        return Result<OrderDto>.Ok(orderDto);
    }

    public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync()
    {
        var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
        var deliveryMethodDtos = _mapper.Map<IReadOnlyList<DeliveryMethod>, IReadOnlyList<DeliveryMethodDto>>(deliveryMethods);
        return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(deliveryMethodDtos);
    }

    public async Task<Result<OrderDto>> UpdateOrderPaymentStatusAsync(string paymentIntentId, OrderStatus status)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        var order = await _unitOfWork.GetRepository<Order, int>().GetEntityWithSpec(spec);

        if (order == null)
        {
            return Result<OrderDto>.Fail(Error.NotFound("Order.NotFound", "Order not found for payment intent"));
        }

        order.Status = status;
        _unitOfWork.GetRepository<Order, int>().Update(order);

        var result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
        {
            return Result<OrderDto>.Fail(Error.InternalServerError("Order.UpdateFailed", "Problem updating order payment status"));
        }

        var orderDto = _mapper.Map<Order, OrderDto>(order);
        return Result<OrderDto>.Ok(orderDto);
    }
}
