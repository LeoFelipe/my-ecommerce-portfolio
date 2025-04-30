using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Orders.Infra.Data;

public class OrderPostgresDbContext(
    DbContextOptions<OrderPostgresDbContext> options,
    IMediatorHandler mediatorHandler,
    INotificationContext notification) : DbContext(options), IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private readonly INotificationContext _notification = notification;

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderPostgresDbContext).Assembly);

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
                entry.Property("CreatedAt").CurrentValue = DateTime.Now;
            }
        }

        var sucess = await base.SaveChangesAsync() > 0;
        if (sucess)
        {
            await _mediatorHandler.PublishEvents(this);
        }
        else
        {
            _notification.Add(
                EnumNotificationType.DATABASE_ERROR,
                "An error occurred while trying to persist data");
        }            

        return sucess;
    }
}