using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Domain.Carts.Entities;

public class Cart
{
    public string Id { get; set; }
    public string ClientName { get; set; }
    public Guid GuidId { get; set; }
}