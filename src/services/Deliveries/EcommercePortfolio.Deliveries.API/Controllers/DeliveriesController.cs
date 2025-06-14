using EcommercePortfolio.Deliveries.API.Application.Queries;
using EcommercePortfolio.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.Deliveries.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveriesController(
    IDeliveryQueries deliveryQuery) : MainController
{
    private readonly IDeliveryQueries _deliveryQuery = deliveryQuery;

    [HttpGet("order/{orderId:guid}", Name = "Get Delivery")]
    public async Task<IActionResult> GetDeliveryByOrderId(Guid orderId)
    {
        var delivery = await _deliveryQuery.GetByOrderId(orderId);
        return OkResponse(delivery);
    }
}
