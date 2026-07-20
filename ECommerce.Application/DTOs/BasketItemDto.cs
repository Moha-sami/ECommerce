using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs;

public class BasketItemDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string ProductName { get; set; } = null!;

    [Required]
    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required]
    public string PictureUrl { get; set; } = null!;

    [Required]
    public string Brand { get; set; } = null!;

    [Required]
    public string Type { get; set; } = null!;
}
