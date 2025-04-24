using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Core.Domain;

public abstract class Entity
{
    private List<Event> _events;
    public IReadOnlyCollection<Event> Events => _events?.AsReadOnly();

    public void AddEvent(Event @event)
    {
        _events ??= [];
        _events.Add(@event);
    }

    public void RemoveEvent(Event eventItem)
    {
        _events?.Remove(eventItem);
    }

    public void ClearEvents()
    {
        _events?.Clear();
    }
}
