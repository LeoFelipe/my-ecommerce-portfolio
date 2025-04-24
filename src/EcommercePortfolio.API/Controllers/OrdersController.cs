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
}
