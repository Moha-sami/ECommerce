using ECommerce.Application.common;
using ECommerce.Application.DTOs.Products;

namespace ECommerce.Application.Contacts;

public interface IProductService
{
    Task<Result<IReadOnlyList<ProductDto>>> GetAllProductsAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default);
    Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default);
    

}
