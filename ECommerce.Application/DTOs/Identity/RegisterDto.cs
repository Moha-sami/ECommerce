using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.Identity;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}
