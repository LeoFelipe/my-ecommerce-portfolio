using EcommercePortfolio.ApiGateways.FakeStoreApi;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Carts.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();

        services.AddHttpClient<IFakeStoreApiService, FakeStoreApiService>("FakeStoreApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(apiSettings.FakeStoreApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());
    }
}
