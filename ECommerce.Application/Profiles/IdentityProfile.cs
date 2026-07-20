using AutoMapper;
using ECommerce.Application.DTOs.Identity;
using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Profiles;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<AppUser, UserDto>();
    }
}
