using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Core.Domain;

public abstract class Entity
{
    private readonly List<Event> _events = [];
    public IReadOnlyCollection<Event> Events => _events?.AsReadOnly();

    public void AddEvent(Event @event) => _events.Add(@event);
    public void RemoveEvent(Event @event) => _events?.Remove(@event);
    public void ClearEvents() => _events?.Clear();
    public bool AnyEventType(Type eventType) => _events.Any(x => x.GetType() == eventType);
}
