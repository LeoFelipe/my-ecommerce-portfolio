using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Infra.ApiServices;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var ApiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();

        services.AddHttpClient<ICartApiService, CartApiService>("CartApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(ApiSettings.CartApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());
    }
}
