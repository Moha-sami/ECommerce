namespace ECommerce.Application.DTOs.Products;

public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string PictureUrl { get; set; } = null!;
    public int ProductBrandId { get; set; }
    public int ProductTypeId { get; set; }
}
