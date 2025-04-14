using EcommercePortfolio.Domain.Products;
using System.Net.Http.Json;

namespace EcommercePortfolio.Infra.ApiServices;

public class FakeStoreApiService(HttpClient _httpClient) : IFakeStoreApiService
{
    public async Task<IEnumerable<string>> GetCategories()
    {
        var response = await _httpClient.GetAsync("/products/categories");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error fetching categories from FakeStore API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<List<string>>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error deserializing categories response", ex);
        }
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await _httpClient.GetAsync($"/products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching product ID: {id} from FakeStore API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<Product>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deserializing product ID: {id} response", ex);
        }
    }

    public async Task<IEnumerable<Product>> GetProducts(int? limit = null)
    {
        var route = "/products";
        if (limit.HasValue)
            route = $"/products?limit={(limit < 1 ? 1 : limit)}";

        var response = await _httpClient.GetAsync(route);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error fetching products from FakeStore API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error deserializing products response", ex);
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
    {
        var response = await _httpClient.GetAsync($"/products/category/{category}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error fetching products from FakeStore API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error deserializing products response", ex);
        }
    }
}
