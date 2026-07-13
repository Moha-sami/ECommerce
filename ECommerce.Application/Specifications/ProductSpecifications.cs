using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Specifications;

namespace ECommerce.Application.Specifications;

public class ProductByIdSpecification : BaseSpecification<Product>
{
    public ProductByIdSpecification(int id) : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
}

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams specParams)
        : base(p =>
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
            (!specParams.TypeId.HasValue || p.TypeId == specParams.TypeId.Value) &&
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search.ToLower())))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
        ApplyPaging(specParams.Skip, specParams.PageSize);

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort.ToLower())
            {
                case "priceasc":
                    AddOrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }
}




