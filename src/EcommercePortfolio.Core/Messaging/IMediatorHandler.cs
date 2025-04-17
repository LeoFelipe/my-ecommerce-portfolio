namespace EcommercePortfolio.Core.Messaging;

public interface IMediatorHandler
{
    Task SendCommand<T>(T command) where T : Command;
}
