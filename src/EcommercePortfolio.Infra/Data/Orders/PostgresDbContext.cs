using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Data.Orders;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty != null)
            {
                idProperty.SetColumnName("id");
                idProperty.IsNullable = false;
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync() > 0;
    }
}