using EcommercePortfolio.Core.Mediator.Messages;
using FluentValidation.Results;

namespace EcommercePortfolio.Core.Mediator;

public interface IMediatorHandler
{
    Task PublishEvent<T>(T evento) where T : Event;
    Task<ValidationResult> SendCommand<T>(T comando) where T : Command;
}
