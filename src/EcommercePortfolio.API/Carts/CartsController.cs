using EcommercePortfolio.API.Carts.Models;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Carts;

[ApiController]
[Route("[controller]")]
public class CartsController(
    ICartRepository cartRepository,
    IMediator mediator) : ControllerBase
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}", Name = "Get Cart by Id")]
    public async Task<IActionResult> GetById(string id)
    {
        var carts = await _cartRepository.GetById(id);
        return Ok(carts);
    }

    [HttpGet("{clientId:guid}", Name = "Get Carts by ClientId")]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        var carts = await _cartRepository.GetByClientId(clientId);
        return Ok(carts);
    }

    [HttpPost(Name = "Create Cart")]
    public async Task<IActionResult> CreateCart(AddCartCommand command)
    {
        await _mediator.Send(command);

        return Created($"{command.ClientId}", command);
    }

    [HttpPut("{id}", Name = "Update Cart")]
    public async Task<IActionResult> UpdateCart(string id, [FromBody] UpdateCartCommand command)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Id cannot be null or empty");

        if (id != command.Id)
            return BadRequest($"Id {id} does not match with command's Id {command.Id}");

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}", Name = "Empty Cart")]
    public async Task<IActionResult> EmptyCart(string id, [FromBody] RemoveCartCommand command)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Id cannot be null or empty");

        if (id != command.Id)
            return BadRequest($"Id {id} does not match with command's Id {command.Id}");

        await _mediator.Send(command);
        return NoContent();
    }
}
