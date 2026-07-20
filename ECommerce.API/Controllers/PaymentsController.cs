using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerce.API.Controllers;

[Authorize]
public class PaymentsController : BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderService _orderService;
    private readonly IConfiguration _config;

    public PaymentsController(
        IPaymentService paymentService,
        IOrderService orderService,
        IConfiguration config)
    {
        _paymentService = paymentService;
        _orderService = orderService;
        _config = config;
    }

    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var result = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        using var reader = new StreamReader(HttpContext.Request.Body);
        var json = await reader.ReadToEndAsync();

        var signatureHeader = Request.Headers["Stripe-Signature"];
        var webhookSecret = _config["StripeSettings:WebhookSecret"];

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, webhookSecret, throwOnApiVersionMismatch: false);
        }
        catch (Exception ex)
        {
            return BadRequest($"Stripe Webhook Signature Verification Failed: {ex.Message}");
        }

        if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
        {
            if (stripeEvent.Data.Object is PaymentIntent intent)
            {
                await _orderService.UpdateOrderPaymentStatusAsync(intent.Id, OrderStatus.PaymentReceived);
            }
        }
        else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
        {
            if (stripeEvent.Data.Object is PaymentIntent intent)
            {
                await _orderService.UpdateOrderPaymentStatusAsync(intent.Id, OrderStatus.PaymentFailed);
            }
        }

        return Ok();
    }
}
