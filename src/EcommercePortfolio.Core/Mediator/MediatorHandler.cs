using EcommercePortfolio.Core.Mediator.Messages;
using FluentValidation.Results;
using MediatR;

namespace EcommercePortfolio.Core.Mediator;

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    private readonly IMediator _mediator = mediator;

    public async Task<ValidationResult> SendCommand<T>(T comando) where T : Command
    {
        return await _mediator.Send(comando);
    }

    public async Task PublishEvent<T>(T evento) where T : Event
    {
        await _mediator.Publish(evento);
    }
}
