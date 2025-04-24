using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Messaging.Mediator;
using EcommercePortfolio.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Data.Orders;

public class PostgresDbContext(
    DbContextOptions<PostgresDbContext> options,
    IMediatorHandler mediatorHandler) : DbContext(options), IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();

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
                entry.Property("CreatedAt").CurrentValue = DateTime.Now;
            }
        }

        var sucess = await base.SaveChangesAsync() > 0;
        if (sucess) await _mediatorHandler.PublishEvents(this);

        return sucess;
    }
}

public static class MediatorExtension
{
    public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var entities = ctx.ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0);

        var entityEvents = entities.SelectMany(x => x.Entity.Events).ToList();

        entities.ToList().ForEach(entity => entity.Entity.ClearEvents());

        var tasks = entityEvents
            .Select(async (entityEvent) =>
            {
                await mediator.PublishEvent(entityEvent);
            });

        await Task.WhenAll(tasks);
    }
}