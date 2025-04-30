using EcommercePortfolio.ExternalServices.FakeStoreApi;
using EcommercePortfolio.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.Carts.API.Controllers;

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
        return OkResponse(categories);
    }

    [HttpGet("{id:int}", Name = "Product")]
    public async Task<IActionResult> GetProducts(int id)
    {
        var product = await _fakeStoreApiService.GetProductById(id);
        return OkResponse(product);
    }

    [HttpGet(Name = "Products")]
    public async Task<IActionResult> GetProducts([FromQuery] int? limit = null)
    {
        var products = await _fakeStoreApiService.GetProducts(limit);
        return OkResponse(products);
    }

    [HttpGet("category/{category}", Name = "Products By Category")]
    public async Task<IActionResult> GetProducts(string category)
    {
        var products = await _fakeStoreApiService.GetProductsByCategory(category);
        return OkResponse(products);
    }
}
