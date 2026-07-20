using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Contacts;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
}
