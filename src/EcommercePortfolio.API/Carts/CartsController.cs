using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using EcommercePortfolio.Infra.Contexts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcommercePortfolio.API.Products;

[ApiController]
[Route("[controller]")]
public class CartsController(MongoDbContext context) : ControllerBase
{
    private readonly MongoDbContext _context = context;

    [HttpGet("clientId:guid", Name = "Get Carts by ClientId")]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        //var carts = await _cartRepository.GetByClientId(clientId);
        return Ok();
    }

    [HttpPost(Name = "Create Cart")]
    public string CreateCart()
    {
        //var cart = new Cart(Guid.CreateVersion7(),
        //[
        //    new(Guid.CreateVersion7(), "Product 1", "Category 1", 2, 10),
        //    new(Guid.CreateVersion7(), "Product 2", "Category 2", 1, 20)
        //]);

        var myGuid = Guid.CreateVersion7();
        var cart = new Cart
        {
            Id = myGuid.ToString(),
            GuidId = myGuid,
            ClientName = "Leo"
        };

        _context.Carts.Add(cart);
        _context.SaveChanges();

        return "mongodb OK!";
    }
}
