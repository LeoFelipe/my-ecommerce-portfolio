using EcommercePortfolio.Carts.Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MongoDb;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace EcommercePortfolio.FunctionalTests.Factories.Configurations;

public class CartFunctionalTestWebApplicationFactory : WebApplicationFactory<Carts.API.Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = BuilderContainerFactory.BuildMongoDbContainer();
    private readonly RedisContainer _redisContainer = BuilderContainerFactory.BuildRedisContainer();
    private readonly RabbitMqContainer _rabbitMqContainer = BuilderContainerFactory.BuildRabbitMqContainer();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var testSettings = new Dictionary<string, string>
            {
                ["ConnectionStrings:RedisCache"] = _redisContainer.GetConnectionString(),
                ["ConnectionStrings:RabbitMq"] = _rabbitMqContainer.GetConnectionString()
            };

            config.AddInMemoryCollection(testSettings);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<MongoDbContext>>();

            services.AddDbContext<MongoDbContext>(options =>
                options.UseMongoDB(_mongoDbContainer.GetConnectionString(), "EcommercePortfolioCart"));
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }
}
