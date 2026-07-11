using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductSController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductSController(IProductService productService)
    {
        _productService = productService;
    }
    //Get all products
    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<ProductDto>>>> GetAllProducts(CancellationToken ct = default)
    {
        var result = await _productService.GetAllProductsAsync(ct);
        if (!result.IsSuccess)
            return NotFound(result);
        return Ok(result);
    }

    //Get product By ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Result<ProductDto>>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await _productService.GetProductByIdAsync(id, ct);
        if (!result.IsSuccess)
            return NotFound(result);
        return Ok(result);
    }
    //Get All Types
    [HttpGet("types")]
    public async Task <ActionResult<Result<IReadOnlyList<TypeDto>>>> GetAllTypes(CancellationToken ct = default)
    {
        var result = await _productService.GetAllTypesAsync(ct);
        if (!result.IsSuccess)
            return NotFound(result);
        return Ok(result);
    }
    //Get All Brands
    [HttpGet("Brand")]
    public async Task<ActionResult<Result<IReadOnlyList<BrandDto>>>> GetAllBrands(CancellationToken ct = default)
    {
        var result = await _productService.GetAllBrandsAsync(ct);
        if (!result.IsSuccess)
            return NotFound(result);
        return Ok(result);
    }
}
