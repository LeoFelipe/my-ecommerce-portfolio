using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.Messaging.Integrations;
using MassTransit;

namespace EcommercePortfolio.Carts.API.Application.Consumers;

public class RemoveCartConsumer(
    IMediatorHandler mediatorHandler) : IConsumer<OrderAuthorizedQueueMessage>
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    public async Task Consume(ConsumeContext<OrderAuthorizedQueueMessage> context)
    {
        try
        {
            await _mediatorHandler.SendCommand(new RemoveCartCommand(context.Message.ClientId));
        }
        catch (Exception ex)
        {
            // TO DO: Log
            var logMessage = ex.Message;
            throw;
        }
    }
}