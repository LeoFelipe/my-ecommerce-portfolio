using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Infra.ApiServices;
using EcommercePortfolio.Services.Configurations;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var externalApiSettings = configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();

        services.AddHttpClient<ICartApiService, CartApiService>("CartApi", httpClient =>
        {
            var baseUrl = isDevelopment
                ? externalApiSettings.CartApiUrl.Replace("https://", "http://").Replace(":5031", ":5030")    
                : externalApiSettings.CartApiUrl;

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
