namespace EcommercePortfolio.Orders.Domain.Entities;

public record struct Product(
    int Id, 
    string Title, 
    string Description, 
    decimal Price, 
    string Category);