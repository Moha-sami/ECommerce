using ECommerce.Application.common;
using ECommerce.Application.Contacts;
using ECommerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    //Get all products
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts(
        [FromQuery] ProductSpecParams specParams, 
        CancellationToken ct = default)
    {
        var result = await _productService.GetAllProductsAsync(specParams, ct);
        return HandleResult(result);
    }

    //Get product By ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await _productService.GetProductByIdAsync(id, ct);
        return HandleResult(result);
    }

    //Get All Types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct = default)
    {
        var result = await _productService.GetAllTypesAsync(ct);
        return HandleResult(result);
    }

    //Get All Brands
    [HttpGet("Brand")]
    public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct = default)
    {
        var result = await _productService.GetAllBrandsAsync(ct);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreateDto productCreateDto, CancellationToken ct = default)
    {
        var result = await _productService.CreateProductAsync(productCreateDto, ct);
        return HandleResult(result);
    }
}
