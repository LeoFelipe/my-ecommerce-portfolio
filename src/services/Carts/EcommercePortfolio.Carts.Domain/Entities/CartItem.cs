namespace EcommercePortfolio.Carts.Domain.Entities;

public class CartItem
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string Category { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public CartItem(int productId, string productName, string category, int quantity, decimal price)
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
