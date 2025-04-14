using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {


        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
