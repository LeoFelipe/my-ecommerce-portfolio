namespace EcommercePortfolio.Orders.Domain.ApiServices;

public record GetCartByClientIdResponse(
    string Id,
    Guid ClientId,
    DateTime CartDate,
    decimal TotalValue,
    IReadOnlyCollection<CartItemDto> CartItems);
