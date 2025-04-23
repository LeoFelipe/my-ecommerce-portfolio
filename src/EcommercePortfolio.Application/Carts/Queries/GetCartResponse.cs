using EcommercePortfolio.Application.Carts.Dtos;
using EcommercePortfolio.Domain.Carts.Entities;

namespace EcommercePortfolio.Application.Carts.Queries;

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

public static class GetCartResponseExtensions
{
    public static IReadOnlyCollection<GetCartResponse> MapToCartsResponse(this IEnumerable<Cart> carts)
        => [.. carts.Select(cart => (GetCartResponse)cart)];
}
