using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Orders.API.Application.Events;

public record OrderAuthorizedEvent(
    Guid Id,
    Guid ClientId) : Event;
