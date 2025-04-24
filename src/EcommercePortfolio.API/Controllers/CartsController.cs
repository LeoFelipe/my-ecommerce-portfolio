using EcommercePortfolio.Application.Carts.Commands;
using EcommercePortfolio.Application.Carts.Queries;
using EcommercePortfolio.Core.Messaging.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CartsController(
    ICartQueries cartQuery,
    IMediatorHandler mediatorHandler) : MainController
{
    private readonly ICartQueries _cartQuery = cartQuery;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    [HttpGet("{id}", Name = "Get Cart by Id")]
    public async Task<IActionResult> GetById(string id)
    {
        var cart = await _cartQuery.GetCartById(id);
        return OkResponse(cart);
    }

    [HttpGet("{clientId:guid}", Name = "Get Carts by ClientId")]
    public async Task<IActionResult> GetCartsByClientId(Guid clientId)
    {
        var carts = await _cartQuery.GetCartsByClientId(clientId);
        return OkResponse(carts);
    }

    [HttpPost(Name = "Create Cart")]
    public async Task<IActionResult> CreateCart(AddCartCommand message)
    {
        await _mediatorHandler.SendCommand(message);
        return Created();
    }

    [HttpPut("{id}", Name = "Update Cart")]
    public async Task<IActionResult> UpdateCart(string id, [FromBody] UpdateCartCommand message)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != message.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {message.Id}");

        await _mediatorHandler.SendCommand(message);

        return NoContent();
    }

    [HttpPut("{id}/item", Name = "Update Cart Item")]
    public async Task<IActionResult> UpdateCartItem(string id, [FromBody] UpdateCartItemCommand message)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != message.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {message.Id}");

        await _mediatorHandler.SendCommand(message);

        return NoContent();
    }

    [HttpDelete("{id}", Name = "Remove Cart")]
    public async Task<IActionResult> RemoveCart(string id, [FromBody] RemoveCartCommand message)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != message.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {message.Id}");

        await _mediatorHandler.SendCommand(message);
        return NoContent();
    }

    [HttpDelete("{id}/item", Name = "Remove Cart Item")]
    public async Task<IActionResult> RemoveCartItem(string id, [FromBody] RemoveCartItemCommand message)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != message.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {message.Id}");

        await _mediatorHandler.SendCommand(message);
        return NoContent();
    }
}
