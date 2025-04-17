using EcommercePortfolio.Core.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace EcommercePortfolio.API.Filters;

public class NotificationFilter(INotificationContext notification) : IAsyncResultFilter
{
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

        if (_notification.Has(EnumNotificationType.VALIDATION_ERROR))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

            var response = CreateResponseError(_notification.Get(EnumNotificationType.VALIDATION_ERROR));
            await context.HttpContext.Response.WriteAsync(response);
            return;
        }
        else if (_notification.Has(EnumNotificationType.NOT_FOUND_ERROR))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = CreateResponseError(_notification.Get(EnumNotificationType.NOT_FOUND_ERROR));
            await context.HttpContext.Response.WriteAsync(response);
            return;
        }
        else if (_notification.HasAnyExcept([EnumNotificationType.INFORMATION]))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = CreateResponseError(_notification.GetAllExcept(EnumNotificationType.INFORMATION));
            await context.HttpContext.Response.WriteAsync(response);
            return;
        }

        _ = await next();
    }

    private string CreateResponseError(IReadOnlyCollection<Notification> notifications)
    {
        var errors = new List<string>();
        foreach (var notificationError in notifications)
        {
            errors.Add(string.IsNullOrWhiteSpace(notificationError.MessageKey)
                ? notificationError.Message
                : $"{notificationError.MessageKey}: {notificationError.Message}");
        }

        return JsonSerializer.Serialize(
            new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", errors.ToArray() }
            }), jsonSerializerOptions);
    }
}