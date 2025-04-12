using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Domain.Carts.Entities;

public class CartItem : Entity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string Category { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public virtual Cart Cart { get; set; }

    public CartItem(Guid productId, string productName, string category, int quantity, decimal price)
    {
        ProductId = productId;
        ProductName = productName;
        Category = category;
        Quantity = quantity;
        Price = price;
    }

    protected CartItem() { }

    internal decimal CalculateTotalAmount() => Quantity * Price;
    internal void AddQuantity(int quantity) => Quantity += quantity;
    internal void UpdateQuantity(int quantity) => Quantity = quantity;
}
