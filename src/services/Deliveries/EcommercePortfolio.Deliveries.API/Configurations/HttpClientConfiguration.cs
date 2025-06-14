using EcommercePortfolio.Deliveries.Domain.ApiServices;
using EcommercePortfolio.Deliveries.Infra.ApiServices;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Deliveries.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();

        services.AddHttpClient<IOrderApiService, OrderApiService>("OrderApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(apiSettings.OrderApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());
    }
}
