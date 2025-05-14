using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Core.Messaging.Integrations;
using MassTransit;

namespace EcommercePortfolio.Carts.API.Application.Consumers;

public class RemoveCartConsumer(
    ILogger<RemoveCartConsumer> logger,
    IMediatorHandler mediatorHandler) : IConsumer<OrderAuthorizedQueueMessage>
{
    private readonly ILogger<RemoveCartConsumer> _logger = logger;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    public async Task Consume(ConsumeContext<OrderAuthorizedQueueMessage> context)
    {
        try
        {
            await _mediatorHandler.SendCommand(new RemoveCartCommand(context.Message.ClientId));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "RemoveCartConsumer - OrderId: {OrderId}, ClientId: {ClientId}",
                context.Message.OrderId,
                context.Message.ClientId);
            throw;
        }
    }
}