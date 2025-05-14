using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Services.ObjectResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace EcommercePortfolio.Services.Filters;

public class NotificationFilter(
    ILogger<NotificationFilter> logger,
    INotificationContext notification) : IAsyncResultFilter
{
    private readonly ILogger<NotificationFilter> _logger = logger;
    private readonly INotificationContext _notification = notification;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        MaxDepth = 5
    };

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        context.HttpContext.Response.ContentType = "application/json";

        if (_notification.Any(EnumNotificationType.VALIDATION_ERROR))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

            var response = CreateResponseError(_notification.Get(EnumNotificationType.VALIDATION_ERROR), StatusCodes.Status422UnprocessableEntity);
            await context.HttpContext.Response.WriteAsync(response);

            _logger.LogWarning(response);

            return;
        }
        else if (_notification.Any(EnumNotificationType.NOT_FOUND_ERROR))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = CreateResponseError(_notification.Get(EnumNotificationType.NOT_FOUND_ERROR), StatusCodes.Status404NotFound);
            await context.HttpContext.Response.WriteAsync(response);

            _logger.LogWarning(response);

            return;
        }
        else if (_notification.AnyExcept([EnumNotificationType.INFORMATION]))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = CreateResponseError(_notification.GetAllExcept(EnumNotificationType.INFORMATION), StatusCodes.Status500InternalServerError);
            await context.HttpContext.Response.WriteAsync(response);

            _logger.LogWarning(response);

            return;
        }

        _ = await next();
    }

    private string CreateResponseError(IReadOnlyCollection<Notification> notifications, int statusCode)
    {
        return JsonSerializer.Serialize(
            new ResponseResult(false, (HttpStatusCode)statusCode, notifications.GetMessagesWithMessageKey()), 
            jsonSerializerOptions);
    }
}