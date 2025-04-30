using EcommercePortfolio.ExternalServices.FakeStoreApi;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Carts.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var externalApiSettings = configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();

        services.AddHttpClient<IFakeStoreApiService, FakeStoreApiService>("FakeStoreApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(externalApiSettings.FakeStoreApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());
    }
}
