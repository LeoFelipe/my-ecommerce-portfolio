using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.Domain.ApiServices;

public interface IFakeStoreApiService
{
    Task<IEnumerable<string>> GetCategories();
    Task<Product> GetProductById(int id);
    Task<IEnumerable<Product>> GetProducts(int? limit = null);
    Task<IEnumerable<Product>> GetProductsByCategory(string category);
}
