using System.Security.Claims;
using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Authorize]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(OrderCreateDto orderDto)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return HandleResult(Result<OrderDto>.Fail(Error.Unauthorized()));
        }

        var result = await _orderService.CreateOrderAsync(
            email,
            orderDto.BasketId,
            orderDto.DeliveryMethodId,
            orderDto.ShipToAddress);

        return HandleResult(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetOrdersForUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return HandleResult(Result<IReadOnlyList<OrderDto>>.Fail(Error.Unauthorized()));
        }

        var result = await _orderService.GetOrdersForUserAsync(email);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetOrderByIdForUser(int id)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return HandleResult(Result<OrderDto>.Fail(Error.Unauthorized()));
        }

        var result = await _orderService.GetOrderByIdForUserAsync(id, email);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpGet("deliveryMethods")]
    public async Task<ActionResult> GetDeliveryMethods()
    {
        var result = await _orderService.GetDeliveryMethodsAsync();
        return HandleResult(result);
    }
}
