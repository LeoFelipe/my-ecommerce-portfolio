using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommercePortfolio.Services.Configurations;

public static class DatabaseExtensions
{
    /// <summary>
    /// Make sure that there is already a Migration created before running the application
    /// Generate migrations using command below:
    /// Nuget package manager: Add-Migration DbInit -context YourDbContextName
    /// </summary>
    public static void ApplyMigration<TDbContext>(this IServiceProvider serviceProvider) where TDbContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }

    public static void AddPostgresDatabase<TDbContext>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string serviceNameForConnectionString) where TDbContext : DbContext
    {
        var conn = configuration.GetConnectionString(serviceNameForConnectionString);
        services.AddDbContextPool<TDbContext>(options =>
            options.UseNpgsql(conn));
    }

    public static void AddMongoDatabase<TDbContext>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string serviceNameForConnectionString, 
        string databaseName) where TDbContext : DbContext
    {
        var conn = configuration.GetConnectionString(serviceNameForConnectionString);
        services.AddDbContextPool<TDbContext>(options =>
            options.UseMongoDB(configuration.GetConnectionString(serviceNameForConnectionString), databaseName));
    }
}
