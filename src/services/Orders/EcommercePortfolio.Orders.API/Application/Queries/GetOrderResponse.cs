using EcommercePortfolio.Orders.API.Application.Dtos;
using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.API.Application.Queries;

public record GetOrderResponse(
    Guid Id,
    Guid ClientId,
    DateTime CreatedAt,
    decimal TotalValue,
    IReadOnlyCollection<OrderItemDto> OrderItems)
{
    public static explicit operator GetOrderResponse(Order order)
    {
        if (order == null || order.Id == Guid.Empty)
            return null;

        return new GetOrderResponse(
            order.Id,
            order.ClientId,
            order.CreatedAt,
            order.TotalValue,
            [.. order.OrderItems.Select(x => (OrderItemDto)x)]);
    }
}

public static class GetOrderResponseExtensions
{
    public static IReadOnlyCollection<GetOrderResponse> MapToOrdersList(this IEnumerable<Order> orders)
        => [.. orders.Select(order => (GetOrderResponse)order)];
}