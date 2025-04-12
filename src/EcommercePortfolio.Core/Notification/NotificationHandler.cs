namespace EcommercePortfolio.Core.Notification;

public class NotificationHandler : INotificationHandler, IDisposable
{
    private readonly List<Notification> _notifications;
    private bool _disposed;

    public NotificationHandler()
    {
        _notifications = [];
        _disposed = false;
    }

    public List<Notification> GetNotifications()
        => [.. _notifications];

    public List<Notification> GetNotificationsBy(EnumNotificationType notificationType)
        => [.. _notifications.Where(n => n.NotificationType == notificationType)];

    public List<Notification> GetNotificationsBy(EnumNotificationType notificationType, string messageKey)
        => [.. _notifications.Where(n => n.NotificationType == notificationType && n.MessageKey == messageKey)];

    public bool HasNotifications()
        => _notifications.Count != 0;

    public bool HasNotifications(EnumNotificationType notificationType)
        => _notifications.Any(n => n.NotificationType == notificationType);

    public bool HasNotifications(EnumNotificationType notificationType, string messageKey)
        => _notifications.Any(n => n.NotificationType == notificationType && n.MessageKey == messageKey);

    public void SetNotification(Notification notification)
        => _notifications.Add(notification);

    public void SetNotification(EnumNotificationType notificationType, string message)
        => _notifications.Add(new Notification(notificationType, message));

    public void SetNotification(EnumNotificationType notificationType, string message, string messageKey)
        => _notifications.Add(new Notification(notificationType, message, messageKey));

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _notifications.Clear();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
