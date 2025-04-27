using EcommercePortfolio.Core.Messaging;
using MediatR;

namespace EcommercePortfolio.Core.Mediator;

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    private readonly IMediator _mediator = mediator;

    public async Task SendCommand<T>(T message) where T : Command
    {
        await _mediator.Send(message);
    }

    public async Task PublishEvent<T>(T message) where T : Event
    {
        await _mediator.Publish(message);
    }
}