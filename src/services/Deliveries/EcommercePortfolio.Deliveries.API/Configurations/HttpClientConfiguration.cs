using EcommercePortfolio.Deliveries.Domain.ApiServices;
using EcommercePortfolio.Deliveries.Infra.ApiServices;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Deliveries.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var externalApiSettings = configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();

        services.AddHttpClient<IOrderApiService, OrderApiService>("OrderApi", httpClient =>
        {
            var baseUrl = isDevelopment
                ? externalApiSettings.OrderApiUrl.Replace("https://", "http://").Replace(":5011", ":5010")
                : externalApiSettings.OrderApiUrl;

            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return isDevelopment
                    ? new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }
                    : new HttpClientHandler();
            })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());
    }
}
