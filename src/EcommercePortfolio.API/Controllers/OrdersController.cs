using EcommercePortfolio.Infra.Data.Orders;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController(PostgresDbContext dbContext) : ControllerBase
    {
        private readonly PostgresDbContext _dbContext = dbContext;

        [HttpGet]
        public IActionResult Get()
        {
            var orders = _dbContext.Orders.ToList();

            return Ok(orders);
        }
    }
}
