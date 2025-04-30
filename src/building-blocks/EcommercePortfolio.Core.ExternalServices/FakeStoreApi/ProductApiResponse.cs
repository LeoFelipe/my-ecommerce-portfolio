namespace EcommercePortfolio.ExternalServices.FakeStoreApi;

public record struct ProductApiResponse(
    int Id, 
    string Title, 
    string Description, 
    decimal Price, 
    string Category);