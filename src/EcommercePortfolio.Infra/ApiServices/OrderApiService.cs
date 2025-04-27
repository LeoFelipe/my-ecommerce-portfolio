using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Deliveries.ApiServices;
using EcommercePortfolio.Domain.Orders.ApiServices;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;

namespace EcommercePortfolio.Infra.ApiServices;

public class OrderApiService(HttpClient _httpClient, INotificationContext notification) : IOrderApiService
{
    private readonly INotificationContext _notification = notification;

    public async Task<GetCartByClientIdResponse> GetOrderById(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/orders/{id}");
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
            switch (ex)
            {
                case SerializationException:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error deserializing order response", "OrderApiService:GetOrderById");
                    break;
                default:
                    _notification.Add(EnumNotificationType.EXCEPTION_ERROR, "Error fetching order", "OrderApiService:GetOrderById");
                    break;
            }

            throw;
        }
    }
}
