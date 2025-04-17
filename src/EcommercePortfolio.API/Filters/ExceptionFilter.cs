using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EcommercePortfolio.API.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger = logger;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        MaxDepth = 5
    };

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
            Status = StatusCodes.Status500InternalServerError,
            Extensions =
            {
                ["traceId"] = logId,
                ["errorMessage"] = string.Join(" --> ", exceptionsTreeList.Select(x => x.Message).ToList()),
                ["stackTrace"] = exception.StackTrace
            }
        };

        _logger.LogError(JsonSerializer.Serialize(problemDetails, jsonSerializerOptions));

        problemDetails.Extensions = null;
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        context.Result = new JsonResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}