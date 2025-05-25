using EcommercePortfolio.Core.Notification;
using System.Runtime.Serialization;

namespace EcommercePortfolio.ApiGateways.MyFakePaymentApi;

public class PaymentApiService(HttpClient _httpClient, INotificationContext notification) : IPaymentApiService
{
    private readonly INotificationContext _notification = notification;

    public async Task<bool> AuthorizePayment(PaymentApiResponse payment)
    {
        try
        {
            var response = await _httpClient.GetAsync("/products/categories"); // mocked endpoint for example
            response.EnsureSuccessStatusCode();

            return await Task.FromResult(response.IsSuccessStatusCode);
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
}
