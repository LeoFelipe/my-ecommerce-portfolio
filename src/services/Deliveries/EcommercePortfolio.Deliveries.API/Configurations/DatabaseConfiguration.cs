using EcommercePortfolio.Deliveries.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Deliveries.API.Configurations;

public static class DatabaseConfiguration
{
    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DeliveryPostgresDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DeliveryPostgresDbContext"));
            options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        });
    }
}
