using EcommercePortfolio.Domain.Orders.ApiServices;

namespace EcommercePortfolio.Domain.Deliveries.ApiServices;

public record GetOrderByIdResponse(
    Guid Id,
    Guid ClientId,
    DateTime CreatedAt,
    decimal TotalValue,
    IReadOnlyCollection<OrderItemDto> CartItems);
