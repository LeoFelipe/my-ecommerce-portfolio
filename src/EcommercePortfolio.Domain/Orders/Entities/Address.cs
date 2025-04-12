namespace EcommercePortfolio.Domain.Orders.Entities;

public class Address
{
    public string ZipCode { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public string BuildingNumber { get; set; }
}
