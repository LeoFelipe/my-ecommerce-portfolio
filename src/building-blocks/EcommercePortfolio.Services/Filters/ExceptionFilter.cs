using System.Text.Json;
using System.Net;
using Microsoft.Extensions.Logging;
using EcommercePortfolio.Services.ObjectResponses;
using EcommercePortfolio.Services.Configurations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EcommercePortfolio.Services.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var exceptionsTreeList = new List<Exception>
        {
            context.Exception
        };

        var exception = context.Exception;
        while (exception.InnerException != null)
        {
            exception = exception.InnerException;
            exceptionsTreeList.Add(exception);
        }

        var logId = Guid.CreateVersion7().ToString();

        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Detail = "Please contact support with the error ID.",
            Instance = context.HttpContext.Request.Path,
            Type = exception.GetType().Name,
            Status = StatusCodes.Status500InternalServerError
        };

        _logger.LogError(JsonSerializer.Serialize(context.Exception, new JsonSerializerOptions().Default()));

        context.HttpContext.Response.StatusCode = problemDetails.Status.Value;

        var response = new ResponseResult(false,  (HttpStatusCode)problemDetails.Status.Value, problemDetails);

        context.Result = new JsonResult(response)
        {
            StatusCode = problemDetails.Status.Value
        };
    }
}