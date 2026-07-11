using ECommerce.Domain.Entities;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class Product:BaseEntity<int>
{

    //[JsonConstructor]
    //public Product(string name, string description, decimal price, int brandId, int typeId)
    //{
    //    if (string.IsNullOrWhiteSpace(name))
    //        throw new ArgumentException("Product name cannot be empty.", nameof(name));

    //    if (price < 0)
    //        throw new ArgumentException("Price cannot be negative.", nameof(price));

    //    Name = name;
    //    Description = description;
    //    Price = price;
    //    BrandId = brandId;
    //    TypeId = typeId;
    //}
    public string Name { get; set; }=null!;
    public string Description { get; set; } = null!;
    public string? PictureUrl { get;  set; }
    public decimal Price { get; set; }
    public ProductBrand ProductBrand { get; set; } = null!;
    public int BrandId { get; set; }
    public ProductType ProductType { get; set; } = null!;
    public int TypeId { get; set; }

}

