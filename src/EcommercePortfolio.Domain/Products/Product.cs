namespace EcommercePortfolio.Domain.Products;

public record struct Product(
    int Id, 
    string Title, 
    string Description, 
    decimal Price, 
    string Category);