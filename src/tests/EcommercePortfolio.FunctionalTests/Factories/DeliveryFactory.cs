using EcommercePortfolio.Deliveries.API.Application.Queries;
using EcommercePortfolio.Services.Configurations;
using System.Net.Http.Json;
using System.Text.Json;

namespace EcommercePortfolio.FunctionalTests.Factories;

public static class DeliveryFactory
{
    public static async Task<GetDeliveryResponse> GetDeliveryByOrderId(HttpClient deliveriesHttpClient, Guid orderId)
    {
        var response = await deliveriesHttpClient.GetAsync($"/deliveries/{orderId}");
        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<ApiResponse<GetDeliveryResponse>>(new JsonSerializerOptions().Default());

        if (apiResponse?.Response == null)
        {
            throw new InvalidOperationException("The API response or its content is null.");
        }

        return apiResponse.Response;
    }
}