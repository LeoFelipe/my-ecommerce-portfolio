using EcommercePortfolio.Orders.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class DatabaseConfiguration
{
    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<OrderPostgresDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OrderPostgresDbContext"));
        });
    }
}
