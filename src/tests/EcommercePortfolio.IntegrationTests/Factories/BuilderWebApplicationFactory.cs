using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace EcommercePortfolio.IntegrationTests.Factories;

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
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:MongoDbConnection"] = mongoConnection,
                        ["ConnectionStrings:RabbitMqConnection"] = rabbitConnection,
                        ["ConnectionStrings:RedisConnection"] = redisConnection
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
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:OrderPostgresDbContext"] = postgresConnection,
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
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:DeliveryPostgresDbContext"] = postgresConnection,
                        ["ConnectionStrings:RabbitMqConnection"] = rabbitConnection,
                        ["ConnectionStrings:RedisConnection"] = redisConnection,
                        ["ApiSettings:OrderApiUrl"] = orderApiUrl
                    };
                    config.AddInMemoryCollection(dict);
                });
            });
    }
}