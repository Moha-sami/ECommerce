namespace ECommerce.Domain.Entities.OrderAggregate;

public class OrderAddress
{
    public OrderAddress()
    {
    }

    public OrderAddress(string firstName, string lastName, string street, string city, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
