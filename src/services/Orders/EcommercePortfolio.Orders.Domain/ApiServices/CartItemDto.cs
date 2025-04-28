namespace EcommercePortfolio.Orders.Domain.ApiServices;

public record CartItemDto(
    int ProductId, 
    string Name, 
    string Category, 
    int Quantity, 
    decimal Price);