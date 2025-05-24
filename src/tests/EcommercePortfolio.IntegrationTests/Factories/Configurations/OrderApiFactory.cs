using EcommercePortfolio.Deliveries.Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public class OrderApiFactory(
    string rabbitMqConnectionString,
    string redisDbConnectionString,
    string postgresDbConnectionString,
    string cartApiUrl) : WebApplicationFactory<Orders.API.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<DeliveryPostgresDbContext>(options =>
                options.UseNpgsql(postgresDbConnectionString));
        });

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.Sources.Clear();

            Environment.SetEnvironmentVariable("DISABLE_ASPIRE_EXPORTERS", "true");

            configBuilder.AddInMemoryCollection(
            [
                new KeyValuePair<string, string>("ConnectionStrings:OrderPostgresDbConnection", postgresDbConnectionString),
                new KeyValuePair<string, string>("ConnectionStrings:RabbitMqConnection", rabbitMqConnectionString),
                new KeyValuePair<string, string>("ConnectionStrings:RedisConnection", redisDbConnectionString),
                new KeyValuePair<string, string>("ApiSettings:CartApiUrl", cartApiUrl),
            ]);
        });
    }
}