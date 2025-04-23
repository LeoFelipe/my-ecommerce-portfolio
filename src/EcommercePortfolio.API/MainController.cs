using EcommercePortfolio.Application;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.API;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected IActionResult OkResponse(object response)
    {
        return Ok(new ResponseResult(true, System.Net.HttpStatusCode.OK, response));
    }

    protected IActionResult BadRequestResponse(object response)
    {
        return BadRequest(new ResponseResult(false, System.Net.HttpStatusCode.BadRequest, response));
    }
}
