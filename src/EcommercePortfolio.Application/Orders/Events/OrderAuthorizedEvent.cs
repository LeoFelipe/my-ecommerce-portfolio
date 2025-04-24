using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Application.Orders.Events;

public record OrderAuthorizedEvent(
    string CartId,
    Guid OrderId,
    Guid ClientId) : Event;
