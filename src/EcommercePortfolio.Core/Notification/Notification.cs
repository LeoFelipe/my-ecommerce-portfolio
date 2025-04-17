using MediatR;

namespace EcommercePortfolio.Core.Notification;

public class Notification
{
    public EnumNotificationType NotificationType { get; }
    public string MessageKey { get; }
    public string Message { get; }

    public Notification(EnumNotificationType notificationType, string message, string messageKey)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        NotificationType = notificationType;
        Message = message;
        MessageKey = messageKey;
    }

    public Notification(EnumNotificationType notificationType, string message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        NotificationType = notificationType;
        Message = message;
    }

    public Notification(string message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        NotificationType = EnumNotificationType.VALIDATION_ERROR;
        Message = message;
    }
}
