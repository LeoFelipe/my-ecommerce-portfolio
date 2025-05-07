using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.Messaging.Integrations;
using EcommercePortfolio.Deliveries.API.Application.Commands;
using MassTransit;

namespace EcommercePortfolio.Deliveries.API.Application.Consumers;

public class CreateDeliveryConsumer(
    IMediatorHandler mediatorHandler) : IConsumer<OrderAuthorizedQueueMessage>
{
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
            // TO DO: Log
            var logMessage = ex.Message;
            throw;
        }
    }
}
