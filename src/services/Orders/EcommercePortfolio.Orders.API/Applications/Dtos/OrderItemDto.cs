using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.API.Applications.Dtos;

public record OrderItemDto(
    int ProductId,
    string Name,
    string Category,
    int Quantity,
    decimal Price)
{
    public static explicit operator OrderItemDto(OrderItem orderItem)
    {
        return new OrderItemDto(
            orderItem.ProductId,
            orderItem.ProductName,
            orderItem.Category,
            orderItem.Quantity,
            orderItem.Price);
    }
}
