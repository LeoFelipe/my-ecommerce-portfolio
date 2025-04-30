using EcommercePortfolio.Carts.API.Application.Commands;
using EcommercePortfolio.Carts.API.Application.Queries;
using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.Carts.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CartsController(
    ICartQueries cartQuery,
    IMediatorHandler mediatorHandler) : MainController
{
    private readonly ICartQueries _cartQuery = cartQuery;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    [HttpGet("{clientId:guid}", Name = "Get Cart by ClientId")]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        var cart = await _cartQuery.GetByClientId(clientId);

        if (cart == null)
            return NotFoundResponse("Cart not found");

        return OkResponse(cart);
    }

    [HttpPost(Name = "Create Cart")]
    public async Task<IActionResult> CreateCart(CreateCartCommand message)
    {
        await _mediatorHandler.SendCommand(message);
        return Created();
    }

    [HttpPut("{clientId:guid}", Name = "Update Cart")]
    public async Task<IActionResult> UpdateCart(Guid clientId, [FromBody] UpdateCartCommand message)
    {
        if (clientId == Guid.Empty)
            return BadRequestResponse("ClientId cannot be null or empty");

        if (clientId != message.ClientId)
            return BadRequestResponse($"ClientId {clientId} does not match with command's ClientId {message.ClientId}");

        await _mediatorHandler.SendCommand(message);

        return NoContent();
    }

    [HttpDelete("{clientId:guid}", Name = "Remove Cart")]
    public async Task<IActionResult> RemoveCart(Guid clientId, [FromBody] RemoveCartCommand message)
    {
        if (clientId == Guid.Empty)
            return BadRequestResponse("ClientId cannot be null or empty");

        if (clientId != message.ClientId)
            return BadRequestResponse($"ClientId {clientId} does not match with command's ClientId {message.ClientId}");

        await _mediatorHandler.SendCommand(message);
        return NoContent();
    }

    [HttpPut("{clientId:guid}/item", Name = "Update Cart Item")]
    public async Task<IActionResult> UpdateCartItem(Guid clientId, [FromBody] UpdateCartItemCommand message)
    {
        if (clientId == Guid.Empty)
            return BadRequestResponse("ClientId cannot be null or empty");

        if (clientId != message.ClientId)
            return BadRequestResponse($"ClientId {clientId} does not match with command's ClientId {message.ClientId}");

        await _mediatorHandler.SendCommand(message);

        return NoContent();
    }

    [HttpDelete("{clientId:guid}/item", Name = "Remove Cart Item")]
    public async Task<IActionResult> RemoveCartItem(Guid clientId, [FromBody] RemoveCartItemCommand message)
    {
        if (clientId == Guid.Empty)
            return BadRequestResponse("ClientId cannot be null or empty");

        if (clientId != message.ClientId)
            return BadRequestResponse($"ClientId {clientId} does not match with command's ClientId {message.ClientId}");

        await _mediatorHandler.SendCommand(message);
        return NoContent();
    }
}
