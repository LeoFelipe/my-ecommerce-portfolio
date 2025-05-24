using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public static class BuilderWebApplicationFactory
{
    public static WebApplicationFactory<Carts.API.Program> BuildCart(
        string rabbitConnection, 
        string redisConnection, 
        string mongoConnection)
    {
        return new WebApplicationFactory<Carts.API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("Environment", "Testing");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.Sources.Clear();

                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:MongoDbConnection"] = mongoConnection,
                        ["ConnectionStrings:RabbitMqConnection"] = rabbitConnection,
                        ["ConnectionStrings:RedisConnection"] = redisConnection,
                        ["ApiSettings:FakeStoreApiUrl"] = "https://fakestoreapi.com"
                    };
                    config.AddInMemoryCollection(dict);
                });
            });
    }

    public static WebApplicationFactory<Orders.API.Program> BuildOrder(
        string rabbitConnection, 
        string redisConnection, 
        string postgresConnection, 
        string cartApiUrl)
    {
        return new WebApplicationFactory<Orders.API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.Sources.Clear();

                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:OrderPostgresDbConnection"] = postgresConnection,
                        ["ConnectionStrings:RabbitMqConnection"] = rabbitConnection,
                        ["ConnectionStrings:RedisConnection"] = redisConnection,
                        ["ApiSettings:CartApiUrl"] = cartApiUrl
                    };
                    config.AddInMemoryCollection(dict);
                });
            });
    }

    public static WebApplicationFactory<Deliveries.API.Program> BuildDelivery(
        string rabbitConnection, 
        string redisConnection, 
        string postgresConnection,
        string orderApiUrl)
    {
        return new WebApplicationFactory<Deliveries.API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.Sources.Clear();

                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:DeliveryPostgresDbConnection"] = postgresConnection,
                        ["ConnectionStrings:RabbitMqConnection"] = rabbitConnection,
                        ["ConnectionStrings:RedisConnection"] = redisConnection,
                        ["ApiSettings:OrderApiUrl"] = orderApiUrl
                    };
                    config.AddInMemoryCollection(dict);
                });
            });
    }
}