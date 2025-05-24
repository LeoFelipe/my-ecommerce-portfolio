using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        IHostEnvironment environment,
        string postgresDbConnection) where TDbContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(postgresDbConnection)
            ?? throw new InvalidOperationException("PostgresDB connection string not found.");

        services.AddDbContextPool<TDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    public static void AddMongoDatabase<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        string mongoDbConnection,
        string databaseName) where TDbContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(mongoDbConnection)
            ?? throw new InvalidOperationException("MongoDB connection string not found.");

        services.AddDbContextPool<TDbContext>(options =>
            options.UseMongoDB(connectionString, databaseName));
    }
}
