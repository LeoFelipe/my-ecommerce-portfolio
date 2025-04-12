using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Domain.Orders.Entities;

public class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string Category { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public virtual Order Order { get; set; }

    public OrderItem(Guid productId, string productName, string category, int quantity, decimal price)
    {
        ProductId = productId;
        ProductName = productName;
        Category = category;
        Quantity = quantity;
        Price = price;
    }

    protected OrderItem() { }

    internal decimal CalculateTotalAmount() => Quantity * Price;
}
