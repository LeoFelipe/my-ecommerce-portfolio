using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.IntegrationTests.Factories;

public static class BuilderContainerFactory
{
    public static MongoDbContainer BuildMongoDbContainer()
    {
        return new MongoDbBuilder()
            .WithImage("mongo:latest")
            .WithName("ecommerceportfolio-mongo-db")
            .WithPortBinding(27017, true)
            .WithUsername("mongo")
            .WithPassword("mongo_password")
            .Build();
    }

    public static PostgreSqlContainer BuildOrderPostgreSqlContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithName("ecommerceportfolio-postgres-db-order")
            .WithPortBinding(5432, true)
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("EcommercePortfolioOrder")
            .Build();
    }

    public static PostgreSqlContainer BuildDeliveryPostgreSqlContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithName("ecommerceportfolio-postgres-db-delivery")
            .WithPortBinding(5432, true)
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("EcommercePortfolioDelivery")
            .Build();
    }

    public static RabbitMqContainer BuildRabbitMqContainer()
    {
        return new RabbitMqBuilder()
            .WithImage("rabbitmq:4.0.8-management-alpine")
            .WithName("ecommerceportfolio-rabbit-mq")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .Build();
    }

    public static RedisContainer BuildRedisContainer()
    {
        return new RedisBuilder()
            .WithImage("redis:latest")
            .WithName("ecommerceportfolio-redis-db")
            .WithPortBinding(6379, true)
            .Build();
    }
}
