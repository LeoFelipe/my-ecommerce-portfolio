IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var postgresdb = builder.AddPostgres("ecommerceportfolio-postgres-db")
    .WithDataVolume()
    .WithPgAdmin();

var mongodb = builder.AddMongoDB("ecommerceportfolio-mongo-db")
    .WithDataVolume();

var redisdb = builder.AddRedis("ecommerceportfolio-redis-db")
    .WithDataVolume();

var rabbitmq = builder.AddRabbitMQ("ecommerceportfolio-rabbit-mq")
    .WithDataVolume()
    .WithManagementPlugin();

builder.AddProject<Projects.EcommercePortfolio_Carts_API>("ecommerceportfolio-carts-api")
    .WithReference(mongodb)
    .WithReference(redisdb)
    .WithReference(rabbitmq);

builder.AddProject<Projects.EcommercePortfolio_Orders_API>("ecommerceportfolio-orders-api")
    .WithReference(postgresdb)
    .WithReference(redisdb)
    .WithReference(rabbitmq);

builder.AddProject<Projects.EcommercePortfolio_Deliveries_API>("ecommerceportfolio-deliveries-api")
    .WithReference(postgresdb)
    .WithReference(redisdb)
    .WithReference(rabbitmq);

await builder.Build().RunAsync();
