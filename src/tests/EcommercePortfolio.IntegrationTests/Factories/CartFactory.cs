using EcommercePortfolio.Orders.Domain.ApiServices;
using System.Net.Http.Json;

namespace EcommercePortfolio.IntegrationTests.Factories;

public static class CartFactory
{
    public static object BuildCartPayload(Guid clientId)
    {
        return new
        {
            clientId,
            cartItems = new[]
            {
                new { productId = 1, name = "Product 01", category = "Category 01", quantity = 10, price = 1 },
                new { productId = 2, name = "Product 02", category = "Category 02", quantity = 20, price = 2 }
            }
        };
    }

    public static async Task PostCart(HttpClient cartsHttpClient, Guid clientId)
    {
        var postCartResponse = await cartsHttpClient.PostAsJsonAsync("/carts", BuildCartPayload(clientId));
        postCartResponse.EnsureSuccessStatusCode();
    }

    public static async Task<GetCartByClientIdResponse> GetCartByClientIdAsync(HttpClient cartsHttpClient, Guid clientId)
    {
        var response = await cartsHttpClient.GetAsync($"/carts/{clientId}");
        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<CartApiResponse<GetCartByClientIdResponse>>();

        if (apiResponse?.Response == null)
        {
            throw new InvalidOperationException("The API response or its content is null.");
        }

        return apiResponse.Response;
    }
}