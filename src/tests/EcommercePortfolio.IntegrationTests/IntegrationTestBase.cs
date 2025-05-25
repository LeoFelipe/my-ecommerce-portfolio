using EcommercePortfolio.IntegrationTests.Factories.Configurations;
using EcommercePortfolio.IntegrationTests.Helpers.Configurations;

namespace EcommercePortfolio.IntegrationTests;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient _cartsHttpClient;
    protected readonly HttpClient _ordersHttpClient;

    protected string _cartMongoConnectionString;
    protected string _orderPostgresConnectionString;
    protected string _deliveryPostgresConnectionString;

    protected IntegrationTestBase()
    {
        _cartsHttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:6050") };
        _ordersHttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:6150") };
    }

    public async Task InitializeAsync()
    {
        await DockerComposeHelper.UpAsync();

        await WaitForService("http://localhost:6050/health");
        await WaitForService("http://localhost:6150/health");

        _cartMongoConnectionString = ConnectionStringFactory.BuildMongoDbString();
        _orderPostgresConnectionString = ConnectionStringFactory.BuildPostgresDbConnectionString("EcommercePortfolioOrder");
        _deliveryPostgresConnectionString = ConnectionStringFactory.BuildPostgresDbConnectionString("EcommercePortfolioDelivery");
    }

    public async Task DisposeAsync()
    {
        _cartsHttpClient.Dispose();
        _ordersHttpClient.Dispose();

        await DockerComposeHelper.DownAsync();
    }

    private static async Task WaitForService(string url, int retries = 20, int delayMs = 1000)
    {
        using var httpClient = new HttpClient();
        for (int i = 0; i < retries; i++)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return;
            }
            catch
            {
                // Ignore temporary error
            }

            await Task.Delay(delayMs);
        }

        throw new InvalidOperationException($"The {url} service did not respond in time.");
    }
}
