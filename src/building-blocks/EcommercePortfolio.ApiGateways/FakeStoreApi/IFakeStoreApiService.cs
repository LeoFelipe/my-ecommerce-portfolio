namespace EcommercePortfolio.ApiGateways.FakeStoreApi;

public interface IFakeStoreApiService
{
    Task<IEnumerable<string>> GetCategories();
    Task<ProductApiResponse> GetProductById(int id);
    Task<IEnumerable<ProductApiResponse>> GetProducts(int? limit = null);
    Task<IEnumerable<ProductApiResponse>> GetProductsByCategory(string category);
}
