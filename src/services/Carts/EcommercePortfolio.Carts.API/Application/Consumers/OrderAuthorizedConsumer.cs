using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Core.Messaging.Integrations;
using MassTransit;
using MassTransit.Mediator;

namespace EcommercePortfolio.Carts.API.Application.Consumers;

public class OrderAuthorizedConsumer(
    IMediator mediator) : IConsumer<RemoveCartQueueMessage>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<RemoveCartQueueMessage> context)
    {
        await _mediator.Send(new RemoveCartCommand(context.Message.ClientId));
    }
}