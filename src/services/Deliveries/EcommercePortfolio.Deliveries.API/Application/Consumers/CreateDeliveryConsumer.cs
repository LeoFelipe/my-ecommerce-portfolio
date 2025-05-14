using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.Messaging.Integrations;
using EcommercePortfolio.Deliveries.API.Application.Commands;
using MassTransit;

namespace EcommercePortfolio.Deliveries.API.Application.Consumers;

public class CreateDeliveryConsumer(
    ILogger<CreateDeliveryConsumer> logger,
    IMediatorHandler mediatorHandler) : IConsumer<OrderAuthorizedQueueMessage>
{
    private readonly ILogger<CreateDeliveryConsumer> _logger = logger;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    public async Task Consume(ConsumeContext<OrderAuthorizedQueueMessage> context)
    {
        try
        {
            await _mediatorHandler.SendCommand(
                new CreateDeliveryCommand(context.Message.OrderId, context.Message.ClientId));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "CreateDeliveryConsumer - OrderId: {OrderId}, ClientId: {ClientId}",
                context.Message.OrderId,
                context.Message.ClientId);
            throw;
        }
    }
}
