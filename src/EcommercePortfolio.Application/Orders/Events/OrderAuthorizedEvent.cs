using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Application.Orders.Events;

public record OrderAuthorizedEvent(Guid ClientId) : Event;
