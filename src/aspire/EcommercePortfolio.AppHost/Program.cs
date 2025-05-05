IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var postgresDb = builder.AddPostgres("ecommerceportfolio-postgres-db")
    .WithDataVolume()
    .WithPgAdmin();

var mongoDb = builder.AddMongoDB("ecommerceportfolio-mongo-db")
    .WithDataVolume()
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
    .WithReference(cartDb)
    .WithReference(redisDb)
    .WithReference(rabbitMq);

var ordersApi = builder.AddProject<Projects.EcommercePortfolio_Orders_API>("ecommerceportfolio-orders-api")
    .WaitFor(postgresDb)
    .WaitFor(redisDb)
    .WaitFor(rabbitMq)
    .WithReference(cartsApi)
    .WithReference(orderDb)
    .WithReference(redisDb)
    .WithReference(rabbitMq);

builder.AddProject<Projects.EcommercePortfolio_Deliveries_API>("ecommerceportfolio-deliveries-api")
    .WaitFor(postgresDb)
    .WaitFor(redisDb)
    .WaitFor(rabbitMq)
    .WithReference(ordersApi)
    .WithReference(deliveryDb)
    .WithReference(redisDb)
    .WithReference(rabbitMq);

await builder.Build().RunAsync();
