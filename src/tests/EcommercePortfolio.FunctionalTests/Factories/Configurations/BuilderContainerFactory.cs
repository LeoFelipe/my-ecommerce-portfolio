using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.FunctionalTests.Factories.Configurations;

public static class BuilderContainerFactory
{
    public static PostgreSqlContainer BuildPostgresDbContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:17.5")
            .WithPortBinding(5432, true)
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("ignore") // a database name is required to run the container
            .Build();
    }

    public static MongoDbContainer BuildMongoDbContainer()
    {
        return new MongoDbBuilder()
            .WithImage("mongo:7.0.5")
            .WithPortBinding(27017, true)
            .WithUsername("mongo")
            .WithPassword("mongo_password")
            .Build();
    }

    public static RedisContainer BuildRedisContainer()
    {
        return new RedisBuilder()
            .WithImage("redis:8.0.0")
            .WithPortBinding(6379, true)
            .Build();
    }

    public static RabbitMqContainer BuildRabbitMqContainer()
    {
        return new RabbitMqBuilder()
            .WithImage("rabbitmq:4.0.9-management")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .Build();
    }
}
