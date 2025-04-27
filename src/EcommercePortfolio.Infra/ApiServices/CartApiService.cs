using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Deliveries.ApiServices;
using EcommercePortfolio.Domain.Orders.ApiServices;
using EcommercePortfolio.Domain.Products;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;

namespace EcommercePortfolio.Infra.ApiServices;

public class CartApiService(HttpClient _httpClient, INotificationContext notification) : ICartApiService
{
    private readonly INotificationContext _notification = notification;

    public async Task<GetCartByClientIdResponse> GetCartByClientId(Guid clientId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/carts/{clientId}");
            response.EnsureSuccessStatusCode();

            var cart = await response.Content
                .ReadFromJsonAsync<CartApiResponse<GetCartByClientIdResponse>>(new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            return cart.Response;
        }
        catch (Exception ex)
        {
            switch(ex)
            {
                case SerializationException:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error deserializing cart response", "CartApiService:GetCartByClientId");
                    break;
                default:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error fetching cart", "CartApiService:GetCartByClientId");
                    break;
            }

            throw;
        }
    }

    public async Task<Product> GetProductById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>();
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

    public async Task<IEnumerable<Product>> GetProducts(int? limit = null)
    {
        try
        {
            var route = "/products";
            if (limit.HasValue)
                route = $"/products?limit={(limit < 1 ? 1 : limit)}";

            var response = await _httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
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

    public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/category/{category}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
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
