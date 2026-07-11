using AutoMapper;
using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services;

internal class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
    {
        var brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync(ct);
        var data = _mapper.Map<IReadOnlyList<BrandDto>>(brands);
        return Result<IReadOnlyList<BrandDto>>.ok(data);
    }

    public async Task<Result<IReadOnlyList<ProductDto>>> GetAllProductsAsync(CancellationToken ct = default)
    {
        var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(ct);
        var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);
        return Result<IReadOnlyList<ProductDto>>.ok(data);
    }

    public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
    {
        var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
        var data = _mapper.Map<IReadOnlyList<TypeDto>>(types);
        return Result<IReadOnlyList<TypeDto>>.ok(data);
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
    {
        var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id, ct);
        if (product == null)
            return Result<ProductDto>.Fail(Error.NotFound("Product Not Found",$"Product with Id {id} Is not Found"));
        var data = _mapper.Map<ProductDto>(product);
        return Result<ProductDto>.ok(data);
    }
}
