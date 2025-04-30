using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Orders.API.Applications.Commands;
using EcommercePortfolio.Orders.API.Applications.Queries;
using EcommercePortfolio.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.Orders.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(
    IOrderQueries orderQuery,
    IMediatorHandler mediatorHandler) : MainController
{
    private readonly IOrderQueries _orderQuery = orderQuery;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    [HttpGet("{id:guid}", Name = "Get Order by id")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderQuery.GetById(id);

        if (order == null)
            return NotFoundResponse("Cart not found");

        return OkResponse(order);
    }

    [HttpGet("client/{clientId:guid}", Name = "Get Orders by ClientId")]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        var orders = await _orderQuery.GetByClientId(clientId);
        return OkResponse(orders);
    }

    [HttpPost(Name = "Create Order")]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand message)
    {
        await _mediatorHandler.SendCommand(message);
        return Created();
    }

}
