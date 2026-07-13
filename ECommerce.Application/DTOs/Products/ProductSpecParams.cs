namespace ECommerce.Application.DTOs.Products;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    
    public string? Sort { get; set; }
    public string? Search { get; set; }
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public int Skip { get; set; } = 0;
    
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
