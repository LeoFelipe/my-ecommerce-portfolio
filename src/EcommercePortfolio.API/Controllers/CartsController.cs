using EcommercePortfolio.Application.Carts.Commands;
using EcommercePortfolio.Application.Carts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CartsController(
    ICartQueries cartQuery,
    IMediator mediator) : MainController
{
    private readonly ICartQueries _cartQuery = cartQuery;
    private readonly IMediator _mediator = mediator;

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
    public async Task<IActionResult> CreateCart(AddCartCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{id}", Name = "Update Cart")]
    public async Task<IActionResult> UpdateCart(string id, [FromBody] UpdateCartCommand command)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != command.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {command.Id}");

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id}/item", Name = "Update Cart Item")]
    public async Task<IActionResult> UpdateCartItem(string id, [FromBody] UpdateCartItemCommand command)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != command.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {command.Id}");

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}", Name = "Remove Cart")]
    public async Task<IActionResult> RemoveCart(string id, [FromBody] RemoveCartCommand command)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != command.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {command.Id}");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}/item", Name = "Remove Cart Item")]
    public async Task<IActionResult> RemoveCartItem(string id, [FromBody] RemoveCartItemCommand command)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequestResponse("Id cannot be null or empty");

        if (id != command.Id)
            return BadRequestResponse($"Id {id} does not match with command's Id {command.Id}");

        await _mediator.Send(command);
        return NoContent();
    }
}
