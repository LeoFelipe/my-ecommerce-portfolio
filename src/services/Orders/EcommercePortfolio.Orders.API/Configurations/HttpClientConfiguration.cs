using EcommercePortfolio.ApiGateways.MyFakePaymentApi;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Infra.ApiServices;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();

        services.AddHttpClient<ICartApiService, CartApiService>("CartApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(apiSettings.CartApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());

        services.AddHttpClient<IPaymentApiService, PaymentApiService>("PaymentApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(apiSettings.PaymentApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());
    }
}
