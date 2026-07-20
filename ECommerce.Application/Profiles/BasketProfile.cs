using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Profiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();
    }
}
