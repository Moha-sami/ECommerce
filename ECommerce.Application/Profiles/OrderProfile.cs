using AutoMapper;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.Application.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderAddress, OrderAddressDto>().ReverseMap();

        CreateMap<DeliveryMethod, DeliveryMethodDto>();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl));

        CreateMap<Order, OrderDto>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
