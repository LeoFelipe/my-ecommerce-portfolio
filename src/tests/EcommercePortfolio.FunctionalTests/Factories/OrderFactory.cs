using EcommercePortfolio.Orders.API.Application.Queries;
using EcommercePortfolio.Services.Configurations;
using System.Net.Http.Json;
using System.Text.Json;

namespace EcommercePortfolio.FunctionalTests.Factories;

public static class OrderFactory
{
    public static object BuildOrderPayload(string cartId, Guid clientId)
    {
        return new
        {
            cartId,
            clientId,
            paymentMethod = "PIX",
            address = new
            {
                zipCode = "49000000",
                state = "SE",
                city = "Aracaju",
                streetAddress = "Rua B",
                numberAddress = 140
            }
        };
    }

    public static async Task<HttpResponseMessage> PostOrder(HttpClient ordersHttpClient, string cartId, Guid clientId)
    {
        return await ordersHttpClient.PostAsJsonAsync("/orders", BuildOrderPayload(cartId, clientId));
    }

    public static async Task<GetOrderResponse> GetOrderByClientId(HttpClient ordersHttpClient, Guid clientId)
    {
        var response = await ordersHttpClient.GetAsync($"/orders/client/{clientId}");
        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<ApiResponse<List<GetOrderResponse>>>(new JsonSerializerOptions().Default());

        if (apiResponse?.Response == null)
        {
            throw new InvalidOperationException("The API response or its content is null.");
        }

        return apiResponse.Response.LastOrDefault();
    }
}