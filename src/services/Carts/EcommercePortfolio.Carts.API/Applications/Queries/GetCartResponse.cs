using EcommercePortfolio.Carts.API.Applications.Dtos;
using EcommercePortfolio.Carts.Domain.Entities;

namespace EcommercePortfolio.Carts.API.Applications.Queries;

public record GetCartResponse(
    string Id,
    Guid ClientId,
    DateTime CartDate,
    decimal TotalValue,
    IReadOnlyCollection<CartItemDto> CartItems)
{
    public static explicit operator GetCartResponse(Cart cart)
    {
        return new GetCartResponse(
            cart.Id,
            cart.ClientId,
            cart.CartDate,
            cart.TotalValue,
            [.. cart.CartItems.Select(x => (CartItemDto)x)]);
    }
}
