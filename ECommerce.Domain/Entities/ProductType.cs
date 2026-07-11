using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class ProductType : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = [];

    //[JsonConstructor]
    //public ProductType(string name)
    //{
    //    if (string.IsNullOrWhiteSpace(name))
    //        throw new ArgumentException("ProductType name cannot be empty.", nameof(name));

    //    Name = name;
    //}
}
