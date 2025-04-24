using MediatR;

namespace EcommercePortfolio.Core.Messaging;

public record Event : Message, INotification
{
}
