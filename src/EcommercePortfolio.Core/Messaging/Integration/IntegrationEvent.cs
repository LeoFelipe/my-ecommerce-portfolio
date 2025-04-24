using MediatR;

namespace EcommercePortfolio.Core.Messaging.Integration;

public record IntegrationEvent : Event, INotification
{
}
