using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Services.Configurations;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;

namespace EcommercePortfolio.Orders.Infra.ApiServices;

public class CartApiService(HttpClient _httpClient, INotificationContext notification) : ICartApiService
{
    private readonly INotificationContext _notification = notification;

    public async Task<GetCartByClientIdResponse> GetCartByClientId(Guid clientId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/carts/{clientId}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var cart = await response.Content
                .ReadFromJsonAsync<CartApiResponse<GetCartByClientIdResponse>>(new JsonSerializerOptions().Default());

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
}
