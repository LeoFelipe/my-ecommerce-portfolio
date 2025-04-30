using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace EcommercePortfolio.Services.Configurations;

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
