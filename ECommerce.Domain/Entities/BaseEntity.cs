namespace ECommerce.Domain.Entities;

public class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }=false;
}
