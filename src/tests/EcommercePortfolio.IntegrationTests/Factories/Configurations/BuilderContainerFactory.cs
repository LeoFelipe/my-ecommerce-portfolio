using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public static class BuilderContainerFactory
{
    private const string NetworkName = "ecommerce_network";

    public static PostgreSqlContainer BuildPostgresDbContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:17.5")
            .WithName("ecommerceportfolio-postgres-db")
            .WithPortBinding(5432, true)
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("ignore") // will be replaced in the actual strings
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-postgres-db")
            .Build();
    }

    public static MongoDbContainer BuildMongoDbContainer()
    {
        return new MongoDbBuilder()
            .WithImage("mongo:7.0.5")
            .WithName("ecommerceportfolio-mongo-db")
            .WithPortBinding(27017, true)
            .WithUsername("mongo")
            .WithPassword("mongo_password")
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-mongo-db")
            .Build();
    }

    public static RedisContainer BuildRedisContainer()
    {
        return new RedisBuilder()
            .WithImage("redis:8.0.0")
            .WithName("ecommerceportfolio-redis-db")
            .WithPortBinding(6379, true)
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-redis-db")
            .Build();
    }

    public static RabbitMqContainer BuildRabbitMqContainer()
    {
        return new RabbitMqBuilder()
            .WithImage("rabbitmq:4.0.9-management")
            .WithName("ecommerceportfolio-rabbit-mq")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-rabbit-mq")
            .Build();
    }

    public static IContainer BuildCartApiContainer(string mongoConnStr, string rabbitConnStr, string redisConnStr)
    {
        return new ContainerBuilder()
            .WithImage("ecommerceportfolio-carts-api")
            .WithName("ecommerceportfolio-carts-api")
            .WithPortBinding(5070, true)
            .WithEnvironment("ConnectionStrings:MongoDbConnection", mongoConnStr)
            .WithEnvironment("ConnectionStrings:RabbitMqConnection", rabbitConnStr)
            .WithEnvironment("ConnectionStrings:RedisConnection", redisConnStr)
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-carts-api")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(req =>
                req.ForPort(5070).ForPath("/health")))
            .Build();
    }

    public static IContainer BuildOrdersApiContainer(string orderDbConnStr, string cartApiUrl, string rabbitConnStr, string redisConnStr)
    {
        return new ContainerBuilder()
            .WithImage("ecommerceportfolio-orders-api")
            .WithName("ecommerceportfolio-orders-api")
            .WithPortBinding(5050, true)
            .WithEnvironment("ConnectionStrings:OrderPostgresDbContext", orderDbConnStr)
            .WithEnvironment("ConnectionStrings:RabbitMqConnection", rabbitConnStr)
            .WithEnvironment("ConnectionStrings:RedisConnection", redisConnStr)
            .WithEnvironment("ApiSettings:CartApiUrl", cartApiUrl)
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-orders-api")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(req =>
                req.ForPort(5050).ForPath("/health")))
            .Build();
    }

    public static IContainer BuildDeliveriesApiContainer(string deliveryDbConnStr, string orderApiUrl, string rabbitConnStr, string redisConnStr)
    {
        return new ContainerBuilder()
            .WithImage("ecommerceportfolio-deliveries-api")
            .WithName("ecommerceportfolio-deliveries-api")
            .WithPortBinding(5060, true)
            .WithEnvironment("ConnectionStrings:DeliveryPostgresDbContext", deliveryDbConnStr)
            .WithEnvironment("ConnectionStrings:RabbitMqConnection", rabbitConnStr)
            .WithEnvironment("ConnectionStrings:RedisConnection", redisConnStr)
            .WithEnvironment("ApiSettings:OrderApiUrl", orderApiUrl)
            .WithNetwork(NetworkName)
            .WithNetworkAliases("ecommerceportfolio-deliveries-api")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(req =>
                req.ForPort(5060).ForPath("/health")))
            .Build();
    }
}
