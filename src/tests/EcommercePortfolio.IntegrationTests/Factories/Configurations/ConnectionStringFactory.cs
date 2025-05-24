using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public static class ConnectionStringFactory
{
    public static string BuildPostgresDbConnectionString(PostgreSqlContainer postgresDbContainer, string dabaseName)
    {
        return $"Host={postgresDbContainer.Hostname};Port={postgresDbContainer.GetMappedPublicPort(5432)};Database={dabaseName};Username=postgres;Password=postgres";
    }

    public static string BuildMongoDbConnectionString(MongoDbContainer mongoDbContainer)
    {
        return $"mongodb://mongo:mongo_password@{mongoDbContainer.Hostname}:{mongoDbContainer.GetMappedPublicPort(27017)}";
    }

    public static string BuildRedisDbConnectionString(RedisContainer redisDbContainer)
    {
        return $"redis://{redisDbContainer.Hostname}:{redisDbContainer.GetMappedPublicPort(6379)}";
    }

    public static string BuildRabbitMqConnectionString(RabbitMqContainer rabbitMqContainer)
    {
        return $"amqp://guest:guest@{rabbitMqContainer.Hostname}:{rabbitMqContainer.GetMappedPublicPort(5672)}";
    }
}
