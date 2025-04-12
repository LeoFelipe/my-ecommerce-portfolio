using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Domain.Carts.Entities;

public class Cart : Entity, IAggregateRoot
{
    public Guid ClientId { get; private set; }
    public DateTime CartDate { get; }
    public decimal TotalValue { get; private set; }

    public List<CartItem> CartItems { get; private set; }


    public Cart(Guid clientId, List<CartItem> cartItems)
    {
        ClientId = clientId;
        CartItems = cartItems;

        CalculateTotalOrderValue();
    }

    protected Cart() { }

    internal bool HasItems(CartItem cartItem)
    {
        return CartItems.Any(x => x.ProductId == cartItem.ProductId);
    }

    internal CartItem GetProductById(Guid productId)
    {
        return CartItems.FirstOrDefault(x => x.ProductId == productId);
    }

    internal void AddItem(CartItem cartItem)
    {
        if (HasItems(cartItem))
        {
            var existingItem = CartItems.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
            existingItem.AddQuantity(cartItem.Quantity);
            CalculateTotalOrderValue();
            return;
        }
        CartItems.Add(cartItem);
        CalculateTotalOrderValue();
    }    

    internal void UpdateQuantity(CartItem cartItem, int quantity)
    {
        cartItem.UpdateQuantity(quantity);
        UpdateItem(cartItem);
    }

    internal void RemoveItem(CartItem cartItem)
    {
        CartItems.Remove(GetProductById(cartItem.ProductId));
        CalculateTotalOrderValue();
    }

    private void UpdateItem(CartItem cartItem)
    {
        var existingItem = GetProductById(cartItem.ProductId);

        CartItems.Remove(existingItem);
        CartItems.Add(cartItem);

        CalculateTotalOrderValue();
    }

    private void CalculateTotalOrderValue()
    {
        var amount = CartItems.Sum(x => x.CalculateTotalAmount());
        TotalValue = amount < 0 ? 0 : amount;
    }
}