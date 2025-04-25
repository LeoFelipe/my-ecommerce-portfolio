using EcommercePortfolio.Application.Orders.Commands;
using EcommercePortfolio.Core.Messaging.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(
    IMediatorHandler mediatorHandler) : MainController
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    [HttpPost(Name = "Add Order")]
    public async Task<IActionResult> AddOrder(AddOrderCommand message)
    {
        await _mediatorHandler.SendCommand(message);
        return Created();
    }


    // TO DO: Create endpoints for GetById, GetByClientId, UpdateOrder, UpdateOrderItem, DeleteOrder, DeleteOrderItem
    // TO DO: Create OrderQueries for GetById, GetByClientId
    // TO DO: Refactor Configuration Files
    // TO DO: Refactor Entity for not instance a new ID on Get register on Database and map the Entity with a different ID
    // TO DO: Configure Logs

}
