using EcommercePortfolio.Services.ObjectResponses;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePortfolio.Services.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected IActionResult OkResponse(object response)
    {
        if (response == null)
            return NotFoundResponse(response);

        return Ok(new ResponseResult(true, System.Net.HttpStatusCode.OK, response));
    }

    protected IActionResult BadRequestResponse(object response)
    {
        return BadRequest(new ResponseResult(false, System.Net.HttpStatusCode.BadRequest, response));
    }

    protected IActionResult NotFoundResponse(object response)
    {
        return NotFound(new ResponseResult(false, System.Net.HttpStatusCode.NotFound, response));
    }
}
