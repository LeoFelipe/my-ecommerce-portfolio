using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Infra.ApiServices;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, ExternalApiSettings externalApiSettings, bool isDevelopment)
    {
        services.AddHttpClient<IFakeStoreApiService, FakeStoreApiService>("FakeStoreApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(externalApiSettings.FakeStoreApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(PollyExtensions.WaitAndRetry());

        services.AddHttpClient<ICartApiService, CartApiService>("CartApi", httpClient =>
        {
            var baseUrl = isDevelopment
                ? externalApiSettings.CartApiUrl.Replace("https://", "http://").Replace(":5061", ":5060")    
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

    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> WaitAndRetry()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                [
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                ], (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Trying for {retryCount} time!");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                });

            return retry;
        }
    }
}
