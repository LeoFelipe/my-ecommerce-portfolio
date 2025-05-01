IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var postgresdb = builder.AddPostgres("ecommerceportfolio-postgres-db")
    .WithDataVolume()
    .WithPgAdmin();

var mongodb = builder.AddMongoDB("ecommerceportfolio-mongo-db")
    .WithDataVolume();

var redisdb = builder.AddRedis("ecommerceportfolio-redis-db");

var rabbitmq = builder.AddRabbitMQ("ecommerceportfolio-rabbit-mq")
    .WithManagementPlugin();

var deliveryDb = postgresdb.AddDatabase("EcommercePortfolioDelivery");
var orderDb = postgresdb.AddDatabase("EcommercePortfolioOrder");
var cartDb = mongodb.AddDatabase("EcommercePortfolioCart");

builder.AddProject<Projects.EcommercePortfolio_Carts_API>("ecommerceportfolio-carts-api")
    .WithReference(cartDb)
    .WithReference(redisdb)
    .WithReference(rabbitmq)
    .WaitFor(mongodb)
    .WaitFor(redisdb)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.EcommercePortfolio_Orders_API>("ecommerceportfolio-orders-api")
    .WithReference(orderDb)
    .WithReference(redisdb)
    .WithReference(rabbitmq)
    .WaitFor(postgresdb)
    .WaitFor(redisdb)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.EcommercePortfolio_Deliveries_API>("ecommerceportfolio-deliveries-api")
    .WithReference(deliveryDb)
    .WithReference(redisdb)
    .WithReference(rabbitmq)
    .WaitFor(postgresdb)
    .WaitFor(redisdb)
    .WaitFor(rabbitmq);

await builder.Build().RunAsync();
