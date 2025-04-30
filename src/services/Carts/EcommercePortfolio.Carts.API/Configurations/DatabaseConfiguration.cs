using EcommercePortfolio.Carts.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Carts.API.Configurations;

public static class DatabaseConfiguration
{
    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<MongoDbContext>(options =>
            options.UseMongoDB(configuration.GetConnectionString("MongoDbConnection"), "ecommerce-portfolio"));
    }
}
