using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Orders.API.Applications.Events;

public record OrderAuthorizedEvent(
    Guid Id,
    Guid ClientId) : Event;
