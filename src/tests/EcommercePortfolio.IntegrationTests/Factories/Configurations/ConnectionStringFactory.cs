namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public static class ConnectionStringFactory
{
    public static string BuildPostgresDbConnectionString(string dabaseName)
    {
        return $"Host=ecommerceportfolio-postgres-db-test;Port=5432;Database={dabaseName};Username=postgres;Password=postgres";
    }

    public static string BuildMongoDbString()
    {
        return $"mongodb://mongo:mongo_password@ecommerceportfolio-mongo-db-test:27017";
    }

    public static string BuildRedisCacheString()
    {
        return $"redis://ecommerceportfolio-redis-db-test:6379";
    }

    public static string BuildRabbitMqString()
    {
        return $"amqp://guest:guest@ecommerceportfolio-rabbit-mq-test:5672/";
    }
}
