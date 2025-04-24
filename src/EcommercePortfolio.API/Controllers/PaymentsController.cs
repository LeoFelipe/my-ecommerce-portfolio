using EcommercePortfolio.Application.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController(
        IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("checkout", Name = "Checkout")]
        public async Task<IActionResult> Checkout(AddOrderCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }
    }
}
