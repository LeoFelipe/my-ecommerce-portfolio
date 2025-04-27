using EcommercePortfolio.Application.Orders.Dtos;
using EcommercePortfolio.Domain.Orders.Entities;

namespace EcommercePortfolio.Application.Orders.Queries;

public record GetOrderResponse(
    Guid Id,
    Guid ClientId,
    DateTime CreatedAt,
    decimal TotalValue,
    IReadOnlyCollection<OrderItemDto> OrderItems)
{
    public static explicit operator GetOrderResponse(Order order)
    {
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