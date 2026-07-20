using ECommerce.Application.common;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Contacts;

public interface IPaymentService
{
    Task<Result<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId);
}
