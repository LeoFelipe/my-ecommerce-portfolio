using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Products;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(
    IFakeStoreApiService fakeStoreApiService) : MainController
{
    private readonly IFakeStoreApiService _fakeStoreApiService = fakeStoreApiService;

    [HttpGet("categories", Name = "Categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _fakeStoreApiService.GetCategories();
        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "Product")]
    public async Task<IActionResult> GetProducts(int id)
    {
        var product = await _fakeStoreApiService.GetProductById(id);
        return Ok(product);
    }

    [HttpGet(Name = "Products")]
    public async Task<IActionResult> GetProducts([FromQuery] int? limit = null)
    {
        var products = await _fakeStoreApiService.GetProducts(limit);
        return Ok(products);
    }

    [HttpGet("category/{category}", Name = "Products By Category")]
    public async Task<IActionResult> GetProducts(string category)
    {
        var products = await _fakeStoreApiService.GetProductsByCategory(category);
        return Ok(products);
    }
}
