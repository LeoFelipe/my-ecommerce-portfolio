using System.Net.Http.Json;

namespace EcommercePortfolio.IntegrationTests.Factories;

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

    public static async Task PostOrder(HttpClient ordersHttpClient, string cartId, Guid clientId)
    {
        var orderResponse = await ordersHttpClient.PostAsJsonAsync("/orders", BuildOrderPayload(cartId, clientId));
        orderResponse.EnsureSuccessStatusCode();
    }
}