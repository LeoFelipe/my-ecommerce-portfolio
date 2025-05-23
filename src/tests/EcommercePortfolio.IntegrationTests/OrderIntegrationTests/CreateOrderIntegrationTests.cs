﻿using EcommercePortfolio.IntegrationTests.Factories;
using EcommercePortfolio.IntegrationTests.Factories.Configurations;
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
    private readonly PostgreSqlContainer _postgresDbContainer;
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
        _postgresDbContainer = BuilderContainerFactory.BuildPostgresDbContainer();
        _rabbitMqContainer = BuilderContainerFactory.BuildRabbitMqContainer();
        _redisContainer = BuilderContainerFactory.BuildRedisContainer();
    }

    public async Task InitializeAsync()
    {
        await DockerNetworkFactory.EnsureNetworkExistsAsync();
        await _mongoDbContainer.StartAsync();
        await _postgresDbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
        await _redisContainer.StartAsync();

        _cartMongoConnectionString = ConnectionStringFactory.BuildMongoDbConnectionString(_mongoDbContainer);
        _orderPostgresConnectionString = ConnectionStringFactory.BuildPostgresDbConnectionString(_postgresDbContainer, "EcommercePortfolioOrder");
        _deliveryPostgresConnectionString = ConnectionStringFactory.BuildPostgresDbConnectionString(_postgresDbContainer, "EcommercePortfolioDelivery");
        var rabbitMqConnectionString = ConnectionStringFactory.BuildRabbitMqConnectionString(_rabbitMqContainer);
        var redisConnectionString = ConnectionStringFactory.BuildRedisDbConnectionString(_redisContainer);

        _cartsApiFactory = new CartApiFactory(rabbitMqConnectionString, redisConnectionString, _cartMongoConnectionString, "EcommercePortfolioCart");
        _cartsHttpClient = _cartsApiFactory.CreateClient();

        _ordersApiFactory = new OrderApiFactory(rabbitMqConnectionString, redisConnectionString, _orderPostgresConnectionString, _cartsHttpClient.BaseAddress.ToString());
        _ordersHttpClient = _ordersApiFactory.CreateClient();

        _deliveriesApiFactory = new DeliveryApiFactory(rabbitMqConnectionString, redisConnectionString, _deliveryPostgresConnectionString, _ordersHttpClient.BaseAddress.ToString());
    }

    public async Task DisposeAsync()
    {
        await _cartsApiFactory.DisposeAsync();
        await _ordersApiFactory.DisposeAsync();
        await _deliveriesApiFactory.DisposeAsync();

        await _mongoDbContainer.DisposeAsync();
        await _postgresDbContainer.DisposeAsync();
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
