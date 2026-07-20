using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Specifications;

namespace ECommerce.Application.Specifications;

public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
{
    public OrderByPaymentIntentIdSpecification(string paymentIntentId)
        : base(o => o.PaymentIntentId == paymentIntentId)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
    }
}
