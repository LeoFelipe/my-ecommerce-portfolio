using EcommercePortfolio.Core.Notification;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.ExternalServices.FakeStoreApi;

public class FakeStoreApiService(HttpClient _httpClient, INotificationContext notification) : IFakeStoreApiService
{
    private readonly INotificationContext _notification = notification;

    public async Task<IEnumerable<string>> GetCategories()
    {
        try
        {
            var response = await _httpClient.GetAsync("/products/categories");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<string>>(new JsonSerializerOptions().Default());
        }
        catch (Exception ex)
        {
            switch(ex)
            {
                case SerializationException:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error deserializing categories response", "FakeStore:GetCategories");
                    break;
                default:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error fetching categories", "FakeStore:GetCategories");
                    break;
            }

            throw;
        }
    }

    public async Task<ProductApiResponse> GetProductById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductApiResponse>(new JsonSerializerOptions().Default());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case SerializationException:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error deserializing response", "FakeStore:GetProductById");
                    break;
                default:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, $"Error fetching product ID: {id}", "FakeStore:GetProductById");
                    break;
            }

            throw;
        }
    }

    public async Task<IEnumerable<ProductApiResponse>> GetProducts(int? limit = null)
    {
        try
        {
            var route = "/products";
            if (limit.HasValue)
                route = $"/products?limit={(limit < 1 ? 1 : limit)}";

            var response = await _httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<ProductApiResponse>>(new JsonSerializerOptions().Default());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case SerializationException:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error deserializing response", "FakeStore:GetProducts");
                    break;
                default:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error fetching products response", "FakeStore:GetProducts");
                    break;
            }

            throw;
        }
    }

    public async Task<IEnumerable<ProductApiResponse>> GetProductsByCategory(string category)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/category/{category}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<ProductApiResponse>>(new JsonSerializerOptions().Default());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case SerializationException:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error deserializing response", "FakeStore:GetCategories");
                    break;
                default:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error fetching products response", "FakeStore:GetCategories");
                    break;
            }

            throw;
        }
    }
}
