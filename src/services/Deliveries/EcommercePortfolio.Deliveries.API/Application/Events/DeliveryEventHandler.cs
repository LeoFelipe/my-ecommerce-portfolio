using MediatR;

namespace EcommercePortfolio.Deliveries.API.Application.Events;

public class DeliveryEventHandler(
    IMediator mediator) :
    INotificationHandler<OrderAuthorizedEvent>
{
}
