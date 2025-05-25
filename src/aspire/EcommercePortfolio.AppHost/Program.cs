IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var postgresDb = builder.AddPostgres("ecommerceportfolio-postgres-db")
    .WithDataVolume("postgres_data")
    .WithPgAdmin();

var mongoDb = builder.AddMongoDB("ecommerceportfolio-mongo-db")
    .WithDataVolume("mongo_data")
    .WithMongoExpress();

var redisDb = builder.AddRedis("ecommerceportfolio-redis-db");

var rabbitMq = builder.AddRabbitMQ("ecommerceportfolio-rabbit-mq")
    .WithManagementPlugin();

var deliveryDb = postgresDb.AddDatabase("EcommercePortfolioDelivery");
var orderDb = postgresDb.AddDatabase("EcommercePortfolioOrder");
var cartDb = mongoDb.AddDatabase("EcommercePortfolioCart");

var cartsApi = builder.AddProject<Projects.EcommercePortfolio_Carts_API>("ecommerceportfolio-carts-api")
    .WaitFor(mongoDb)
    .WaitFor(redisDb)
    .WaitFor(rabbitMq)
    .WithReference(cartDb, "MongoDb")
    .WithReference(redisDb, "RedisCache")
    .WithReference(rabbitMq, "RabbitMq");

var ordersApi = builder.AddProject<Projects.EcommercePortfolio_Orders_API>("ecommerceportfolio-orders-api")
    .WaitFor(postgresDb)
    .WaitFor(redisDb)
    .WaitFor(rabbitMq)
    .WithReference(orderDb, "PostgresDb")
    .WithReference(redisDb, "RedisCache")
    .WithReference(rabbitMq, "RabbitMq")
    .WithEnvironment("ApiSettings__CartApiUrl", () => cartsApi.GetEndpoint("http").Url);

builder.AddProject<Projects.EcommercePortfolio_Deliveries_API>("ecommerceportfolio-deliveries-api")
    .WaitFor(postgresDb)
    .WaitFor(redisDb)
    .WaitFor(rabbitMq)
    .WithReference(deliveryDb, "PostgresDb")
    .WithReference(redisDb, "RedisCache")
    .WithReference(rabbitMq, "RabbitMq")
    .WithEnvironment("ApiSettings__OrderApiUrl", () => ordersApi.GetEndpoint("http").Url);

await builder.Build().RunAsync();
