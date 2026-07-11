using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities
{
    public class ProductBrand: BaseEntity<int>
    {
        
        public string Name { get;  set; } = null!;
        public ICollection<Product> Products { get;  set; } = [];

        //[JsonConstructor]
        //public Brand(string name)
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //        throw new ArgumentException("Brand name cannot be empty.", nameof(name));

        //    Name = name;
        //}
    }

}
