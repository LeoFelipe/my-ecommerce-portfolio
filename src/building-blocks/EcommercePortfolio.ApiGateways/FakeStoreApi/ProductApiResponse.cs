namespace EcommercePortfolio.ApiGateways.FakeStoreApi;

public record struct ProductApiResponse(
    int Id, 
    string Title, 
    string Description, 
    decimal Price, 
    string Category);