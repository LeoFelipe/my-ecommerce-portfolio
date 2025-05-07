using EcommercePortfolio.Core.Domain;
namespace EcommercePortfolio.Orders.Domain.Entities;

public class OrderItem : SqlEntity
{
    public Guid OrderId { get; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string Category { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public virtual Order Order { get; set; }

    private OrderItem(int productId, string productName, string category, int quantity, decimal price)
    {
        ProductId = productId;
        ProductName = productName;
        Category = category;
        Quantity = quantity;
        Price = price;
    }

    public OrderItem() { }

    internal decimal CalculateTotalAmount() => Quantity * Price;

    public static OrderItem CreateOrderItem(int productId, string productName, string category, int quantity, decimal price)
    {
        if (productId <= 0) throw new DomainException("Product id not informed");
        if (string.IsNullOrWhiteSpace(productName)) throw new DomainException("Product name not informed");
        if (string.IsNullOrWhiteSpace(category)) throw new DomainException("Category not informed");
        if (quantity <= 0) throw new DomainException("Quantity not informed");
        if (price <= 0) throw new DomainException("Price not informed");

        return new OrderItem(productId, productName, category, quantity, price);
    }
}
