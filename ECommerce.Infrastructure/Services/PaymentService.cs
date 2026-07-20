using AutoMapper;
using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = ECommerce.Domain.Entities.Product;

namespace ECommerce.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public PaymentService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IConfiguration config,
        IMapper mapper)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _config = config;
        _mapper = mapper;
    }

    public async Task<Result<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null)
        {
            return Result<CustomerBasketDto>.Fail(Error.NotFound("Basket.NotFound", "Customer basket was not found"));
        }

        decimal shippingPrice = 0m;
        if (basket.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod != null)
            {
                shippingPrice = deliveryMethod.Price;
                basket.ShippingPrice = deliveryMethod.Price;
            }
        }

        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
            if (productItem != null)
            {
                item.Price = productItem.Price;
            }
        }

        var service = new PaymentIntentService();
        PaymentIntent intent;

        var subtotal = basket.Items.Sum(i => i.Quantity * i.Price);
        var totalAmount = (long)((subtotal + shippingPrice) * 100);

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = totalAmount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            intent = await service.CreateAsync(options);
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = totalAmount
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketRepository.UpdateBasketAsync(basket);

        var basketDto = _mapper.Map<CustomerBasketDto>(basket);
        return Result<CustomerBasketDto>.Ok(basketDto);
    }
}
