using EcommercePortfolio.Deliveries.Domain.ApiServices;

namespace EcommercePortfolio.Deliveries.Domain.ApiServices;

public record GetOrderByIdResponse(
    Guid Id,
    Guid ClientId,
    DateTime CreatedAt,
    decimal TotalValue);
