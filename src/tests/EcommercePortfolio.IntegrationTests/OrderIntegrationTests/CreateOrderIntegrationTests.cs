using EcommercePortfolio.IntegrationTests.AssertsHelper;
using EcommercePortfolio.IntegrationTests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace EcommercePortfolio.IntegrationTests.OrderIntegrationTests;

public class CreateOrderIntegrationTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Carts.API.Program> _cartsApiFactory;
    private readonly WebApplicationFactory<Orders.API.Program> _ordersApiFactory;
    private readonly WebApplicationFactory<Deliveries.API.Program> _deliveriesApiFactory;
    private readonly MongoDbContainer _mongoDbContainer;
    private readonly PostgreSqlContainer _postgresDbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;

    private HttpClient _cartsHttpClient;
    private HttpClient _ordersHttpClient;

    public CreateOrderIntegrationTests()
    {
        _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:7")
            .WithPortBinding(27017, true)
            .Build();

        _postgresDbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithPortBinding(5432, true)
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("ordersdb")
            .Build();

        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .Build();

        _cartsHttpClient = null!;
        _ordersHttpClient = null!;

        var mongoConnection = _mongoDbContainer.GetConnectionString();
        var postgresConnection = _postgresDbContainer.GetConnectionString();
        var rabbitConnection = _rabbitMqContainer.GetConnectionString();

        _cartsApiFactory = new WebApplicationFactory<Carts.API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:ecommerceportfolio-mongo-db"] = mongoConnection,
                        ["ConnectionStrings:ecommerceportfolio-rabbit-mq"] = rabbitConnection
                    };
                    config.AddInMemoryCollection(dict);
                });
            });

        _ordersApiFactory = new WebApplicationFactory<Orders.API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:ecommerceportfolio-postgres-db"] = postgresConnection,
                        ["ConnectionStrings:ecommerceportfolio-rabbit-mq"] = rabbitConnection
                    };
                    config.AddInMemoryCollection(dict);
                });
            });

        _deliveriesApiFactory = new WebApplicationFactory<Deliveries.API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:ecommerceportfolio-postgres-db"] = postgresConnection,
                        ["ConnectionStrings:ecommerceportfolio-rabbit-mq"] = rabbitConnection
                    };
                    config.AddInMemoryCollection(dict);
                });
            });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        await _postgresDbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _cartsHttpClient = new HttpClient(handler) { BaseAddress = _cartsApiFactory.Server.BaseAddress };
        _ordersHttpClient = new HttpClient(handler) { BaseAddress = _ordersApiFactory.Server.BaseAddress };
    }

    public async Task DisposeAsync()
    {
        await _cartsApiFactory.DisposeAsync();
        await _ordersApiFactory.DisposeAsync();
        await _deliveriesApiFactory.DisposeAsync();

        await _mongoDbContainer.DisposeAsync();
        await _mongoDbContainer.DisposeAsync();
        await _postgresDbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }

    [Fact]
    public async Task OrderIntegration_CreateOrder_ShouldCreateOrderAndCreateDeliveryAndDeleteCart()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act 1: Create the Cart
        await CartFactory.PostCart(_cartsHttpClient, clientId);
        var getCartResponse = await CartFactory.GetCartByClientIdAsync(_cartsHttpClient, clientId);

        // Act 2: Create the Order
        await OrderFactory.PostOrder(_ordersHttpClient, getCartResponse.Id, clientId);

        // Asserts
        await MongoAssertHelper.AssertCartDoesNotExistAsync(_mongoDbContainer.GetConnectionString(), clientId);
        await PostgresAssertHelper.AssertOrderExistsAsync(_postgresDbContainer.GetConnectionString(), clientId);
        await PostgresAssertHelper.AssertDeliveryExistsAsync(_postgresDbContainer.GetConnectionString(), clientId);
    }
}
