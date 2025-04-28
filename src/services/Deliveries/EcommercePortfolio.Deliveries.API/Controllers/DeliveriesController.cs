using EcommercePortfolio.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.Deliveries.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveriesController : MainController
{
    [HttpGet(Name = "Get Delivery")]
    public IActionResult GetDelivery()
    {
        return Ok("Delivered");
    }
}
