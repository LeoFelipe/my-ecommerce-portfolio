using MediatR;

namespace EcommercePortfolio.Core.Mediator.Messages;

public class Event : INotification
{
    public DateTime Timestamp { get; private set; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}
