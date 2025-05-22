using EcommercePortfolio.IntegrationTests.Factories;
using EcommercePortfolio.IntegrationTests.Helpers.Asserts;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.IntegrationTests.OrderIntegrationTests;

public class CreateOrderIntegrationTests : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer;
    private readonly PostgreSqlContainer _orderPostgresDbContainer;
    private readonly PostgreSqlContainer _deliveryPostgresDbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;
    private readonly RedisContainer _redisContainer;

    private WebApplicationFactory<Carts.API.Program> _cartsApiFactory = null!;
    private WebApplicationFactory<Orders.API.Program> _ordersApiFactory = null!;
    private WebApplicationFactory<Deliveries.API.Program> _deliveriesApiFactory = null!;

    private string _cartMongoConnectionString;
    private string _orderPostgresConnectionString;
    private string _deliveryPostgresConnectionString;

    private HttpClient _cartsHttpClient = null!;
    private HttpClient _ordersHttpClient = null!;

    public CreateOrderIntegrationTests()
    {
        _mongoDbContainer = BuilderContainerFactory.BuildMongoDbContainer();
        _orderPostgresDbContainer = BuilderContainerFactory.BuildOrderPostgreSqlContainer();
        _deliveryPostgresDbContainer = BuilderContainerFactory.BuildDeliveryPostgreSqlContainer();
        _rabbitMqContainer = BuilderContainerFactory.BuildRabbitMqContainer();
        _redisContainer = BuilderContainerFactory.BuildRedisContainer();
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        await _orderPostgresDbContainer.StartAsync();
        await _deliveryPostgresDbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
        await _redisContainer.StartAsync();

        _cartMongoConnectionString = _mongoDbContainer.GetConnectionString();
        _orderPostgresConnectionString = _orderPostgresDbContainer.GetConnectionString();
        _deliveryPostgresConnectionString = _deliveryPostgresDbContainer.GetConnectionString();
        var rabbitConnection = _rabbitMqContainer.GetConnectionString();
        var redisConnection = _redisContainer.GetConnectionString();

        _cartsApiFactory = BuilderWebApplicationFactory
            .BuildCart(rabbitConnection, redisConnection, _cartMongoConnectionString);
        _cartsHttpClient = _cartsApiFactory.CreateClient();

        _ordersApiFactory = BuilderWebApplicationFactory
            .BuildOrder(rabbitConnection, redisConnection, _orderPostgresConnectionString, _cartsApiFactory.Server.BaseAddress.ToString());
        _ordersHttpClient = _ordersApiFactory.CreateClient();

        _deliveriesApiFactory = BuilderWebApplicationFactory
            .BuildDelivery(rabbitConnection, redisConnection, _deliveryPostgresConnectionString, _ordersApiFactory.Server.BaseAddress.ToString());

    }

    public async Task DisposeAsync()
    {
        await _cartsApiFactory.DisposeAsync();
        await _ordersApiFactory.DisposeAsync();
        await _deliveriesApiFactory.DisposeAsync();

        await _mongoDbContainer.DisposeAsync();
        await _orderPostgresDbContainer.DisposeAsync();
        await _deliveryPostgresDbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
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
        await MongoAssertHelper.AssertCartDoesNotExistAsync(_cartMongoConnectionString, clientId);
        await PostgresAssertHelper.AssertOrderExistsAsync(_orderPostgresConnectionString, clientId);
        await PostgresAssertHelper.AssertDeliveryExistsAsync(_deliveryPostgresConnectionString, clientId);
    }
}
