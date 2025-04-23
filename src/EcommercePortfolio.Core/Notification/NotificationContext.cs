using FluentValidation.Results;

namespace EcommercePortfolio.Core.Notification;

public class NotificationContext : INotificationContext, IDisposable
{
    private readonly List<Notification> _notifications;
    private bool _disposed;

    public NotificationContext()
    {
        _notifications = [];
        _disposed = false;
    }

    public bool Any()
        => _notifications.Count != 0;

    public bool Any(EnumNotificationType notificationType)
        => _notifications.Any(n => n.NotificationType == notificationType);

    public bool Any(EnumNotificationType notificationType, string messageKey)
        => _notifications.Any(n => n.NotificationType == notificationType && n.MessageKey == messageKey);

    public bool AnyExcept(EnumNotificationType notificationType)
        => Any() && !_notifications.Any(n => n.NotificationType == notificationType);

    public bool AnyExcept(IReadOnlyCollection<EnumNotificationType> notificationsType)
        => Any() && !_notifications.Any(n => notificationsType.Contains(n.NotificationType));

    public IReadOnlyCollection<Notification> Get()
        => _notifications.AsReadOnly();

    public IReadOnlyCollection<Notification> Get(EnumNotificationType notificationType)
        => _notifications.Where(n => n.NotificationType == notificationType).ToList().AsReadOnly();

    public IReadOnlyCollection<Notification> Get(EnumNotificationType notificationType, string messageKey)
        => _notifications.Where(n => n.NotificationType == notificationType && n.MessageKey == messageKey).ToList().AsReadOnly();

    public IReadOnlyCollection<Notification> GetAllExcept(EnumNotificationType notificationType)
        => _notifications.Where(n => n.NotificationType != notificationType).ToList().AsReadOnly();

    public IReadOnlyCollection<Notification> GetAllExcept(IReadOnlyCollection<EnumNotificationType> notificationsType)
        => _notifications.Where(n => !notificationsType.Contains(n.NotificationType)).ToList().AsReadOnly();

    public IReadOnlyCollection<string> GetOnlyMessages()
        => _notifications.Select(x => x.Message).ToList().AsReadOnly();

    public IReadOnlyCollection<string> GetOnlyMessages(EnumNotificationType notificationType)
        => _notifications.Where(n => n.NotificationType == notificationType).Select(x => x.Message).ToList().AsReadOnly();

    public IReadOnlyCollection<string> GetOnlyMessages(EnumNotificationType notificationType, string messageKey)
        => _notifications.Where(n => n.NotificationType == notificationType && n.MessageKey == messageKey).Select(x => x.Message).ToList().AsReadOnly();

    public void Add(Notification notification)
        => _notifications.Add(notification);

    public void Add(EnumNotificationType notificationType, string message)
        => Add(new Notification(notificationType, message));

    public void Add(EnumNotificationType notificationType, string message, string messageKey)
        => Add(new Notification(notificationType, message, messageKey));

    public void Add(EnumNotificationType notificationType, ValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        foreach (var error in validationResult.Errors)
        {
            _notifications.Add(new Notification(notificationType, error.ErrorMessage, error.PropertyName));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _notifications.Clear();
        }

        _disposed = true;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public static class NotificationContextExtensions
{
    public static IReadOnlyCollection<string> GetMessagesWithMessageKey(this IReadOnlyCollection<Notification> notifications)
    {
        var errors = new List<string>();
        foreach (var notificationError in notifications)
        {
            errors.Add(string.IsNullOrWhiteSpace(notificationError.MessageKey)
                ? notificationError.Message
                : $"{notificationError.MessageKey}: {notificationError.Message}");
        }

        return errors.AsReadOnly();
    }
}
