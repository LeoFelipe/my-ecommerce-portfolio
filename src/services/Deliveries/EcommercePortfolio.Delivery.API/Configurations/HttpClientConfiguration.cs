using EcommercePortfolio.Deliveries.Domain.ApiServices;
using EcommercePortfolio.Deliveries.Infra.ApiServices;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace EcommercePortfolio.Deliveries.API.Configurations;

public static class HttpClientConfiguration
{
    public static void AddHttpClientConfiguration(this IServiceCollection services, ExternalApiSettings externalApiSettings, bool isDevelopment)
    {
        services.AddHttpClient<IOrderApiService, OrderApiService>("OrderApi", httpClient =>
        {
            var baseUrl = isDevelopment
                ? externalApiSettings.CartApiUrl.Replace("https://", "http://").Replace(":5051", ":5050")
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
