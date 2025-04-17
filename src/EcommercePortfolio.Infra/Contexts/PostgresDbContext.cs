using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Orders.Entities;
using EcommercePortfolio.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Contexts;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
}