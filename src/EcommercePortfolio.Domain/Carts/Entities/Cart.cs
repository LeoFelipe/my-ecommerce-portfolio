using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Domain.Carts.Entities;

public class Cart : NoSqlEntity, IAggregateRoot
{
    public Guid ClientId { get; private set; }
    public DateTime CartDate { get; }
    public DateTime ExpirationDate { get; }
    public decimal TotalValue { get; private set; }

    public List<CartItem> CartItems { get; private set; }


    public Cart(Guid clientId, List<CartItem> cartItems)
    {
        ClientId = clientId;
        CartItems = cartItems;
        CartDate = DateTime.Now;
        ExpirationDate = CartDate.AddDays(7);

        CalculateTotalOrderValue();
    }

    protected Cart() { }

    public bool HasItems(CartItem cartItem)
    {
        return HasItems(cartItem.ProductId);
    }

    public bool HasItems(int productId)
    {
        return CartItems.Any(x => x.ProductId == productId);
    }

    public CartItem GetProductById(int productId)
    {
        return CartItems.SingleOrDefault(x => x.ProductId == productId);
    }

    public void AddItem(CartItem cartItem)
    {
        if (HasItems(cartItem))
        {
            var existingItem = GetProductById(cartItem.ProductId);
            existingItem.AddQuantity(cartItem.Quantity);
            CalculateTotalOrderValue();
            return;
        }
        CartItems.Add(cartItem);
        CalculateTotalOrderValue();
    }

    public void UpdateQuantity(CartItem cartItem, int quantity)
    {
        cartItem.UpdateQuantity(quantity);
        UpdateItem(cartItem);
    }

    public void RemoveItem(CartItem cartItem)
    {
        RemoveItem(cartItem.ProductId);
    }

    public void RemoveItem(int productId)
    {
        CartItems.Remove(GetProductById(productId));
        CalculateTotalOrderValue();
    }

    public void UpdateItem(CartItem cartItem)
    {
        if (HasItems(cartItem))
        {
            var existingItem = GetProductById(cartItem.ProductId);

            CartItems.Remove(existingItem);
            CartItems.Add(cartItem);

            CalculateTotalOrderValue();
        }
    }

    public void UpdateAllItems(List<CartItem> newCartItems)
    {
        CartItems.Clear();
        CartItems.AddRange(newCartItems);

        CalculateTotalOrderValue();
    }

    private void CalculateTotalOrderValue()
    {
        var amount = CartItems.Sum(x => x.CalculateTotalAmount());
        TotalValue = amount < 0 ? 0 : amount;
    }
}