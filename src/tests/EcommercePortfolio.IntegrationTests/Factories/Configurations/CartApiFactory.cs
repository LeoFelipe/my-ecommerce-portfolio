using EcommercePortfolio.Carts.Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public class CartApiFactory(
    string rabbitMqConnectionString,
    string redisDbConnectionString,
    string mongoDbConnectionString,
    string databaseName) : WebApplicationFactory<Carts.API.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<MongoDbContext>(options =>
                options.UseMongoDB(mongoDbConnectionString, databaseName));
        });

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.Sources.Clear();

            Environment.SetEnvironmentVariable("DISABLE_ASPIRE_EXPORTERS", "true");

            configBuilder.AddInMemoryCollection(
            [
                new KeyValuePair<string, string>("ConnectionStrings:MongoDbConnection", mongoDbConnectionString),
                new KeyValuePair<string, string>("ConnectionStrings:RabbitMqConnection", rabbitMqConnectionString),
                new KeyValuePair<string, string>("ConnectionStrings:RedisConnection", redisDbConnectionString),
                new KeyValuePair<string, string>("ApiSettings:FakeStoreApiUrl", "https://fakestoreapi.com"),
            ]);
        });
    }
}