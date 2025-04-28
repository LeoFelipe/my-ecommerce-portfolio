using EcommercePortfolio.Orders.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Orders.API.Configurations;

public static class DbMigrationConfiguration
{
    /// <summary>
    /// Make sure that there is already a Migration created before running the application
    /// Generate migrations using command below:
    /// Nuget package manager: Add-Migration DbInit -context YourDbContextName
    /// </summary>
    public static void Configure(WebApplication app)
    {
        var serviceProvider = app.Services.CreateScope().ServiceProvider;

        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        ApplyMigration<OrderPostgresDbContext>(scope);
    }

    public static void ApplyMigration<TDbContext>(IServiceScope scope) where TDbContext : DbContext
    {
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }
}
