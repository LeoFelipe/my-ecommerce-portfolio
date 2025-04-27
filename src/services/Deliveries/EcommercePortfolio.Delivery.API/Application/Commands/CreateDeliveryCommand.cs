using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Deliveries.API.Application.Commands;

public record CreateDeliveryCommand(
    Guid OrderId,
    Guid ClientId) : Command;