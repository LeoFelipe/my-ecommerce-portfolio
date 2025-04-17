using MediatR;

namespace EcommercePortfolio.Core.Messaging;

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    private readonly IMediator _mediator = mediator;

    public async Task SendCommand<T>(T command) where T : Command
    {
        await _mediator.Send(command);
    }
}