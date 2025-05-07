using EcommercePortfolio.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Core.Mediator;

public static class MediatorExtension
{
    public static async Task PublishEvents<T>(this IMediatorHandler mediator, T dbContext) where T : DbContext
    {
        var entities = dbContext.ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0);

        var entityEvents = entities.SelectMany(x => x.Entity.Events).ToList();

        entities.ToList().ForEach(entity => entity.Entity.ClearEvents());

        if (entityEvents.Count > 0)
        {
            var tasks = entityEvents
                .Select(async (entityEvent) =>
                {
                    await mediator.PublishEvent(entityEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}