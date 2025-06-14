using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Deliveries.Infra.Data;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.FunctionalTests.Factories.Configurations;

public class OrderFunctionalTestWebApplicationFactory : WebApplicationFactory<Orders.API.Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = BuilderContainerFactory.BuildMongoDbContainer();
    private readonly PostgreSqlContainer _postgresDbContainer = BuilderContainerFactory.BuildPostgresDbContainer();
    private readonly RedisContainer _redisContainer = BuilderContainerFactory.BuildRedisContainer();
    private readonly RabbitMqContainer _rabbitMqContainer = BuilderContainerFactory.BuildRabbitMqContainer();

    public Mock<ICartApiService> CartApiServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<MongoDbContext>>();
            services.RemoveAll<DbContextOptions<DeliveryPostgresDbContext>>();
            services.RemoveAll<DbContextOptions<OrderPostgresDbContext>>();
            services.RemoveAll<ICartApiService>();
            services.AddSingleton(CartApiServiceMock.Object);

            services.AddDbContext<OrderPostgresDbContext>(options =>
                options.UseNpgsql(_postgresDbContainer.GetConnectionString().Replace("Database=ignore", "Database=EcommercePortfolioOrder")));

            services.AddDbContext<DeliveryPostgresDbContext>(options =>
                options.UseNpgsql(_postgresDbContainer.GetConnectionString().Replace("Database=ignore", "Database=EcommercePortfolioDelivery")));
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        await _postgresDbContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
        await _postgresDbContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }
}
