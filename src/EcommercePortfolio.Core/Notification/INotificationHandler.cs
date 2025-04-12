namespace EcommercePortfolio.Core.Notification;

public interface INotificationHandler
{
    bool HasNotifications();
    bool HasNotifications(EnumNotificationType notificationType);
    bool HasNotifications(EnumNotificationType notificationType, string messageKey);

    List<Notification> GetNotifications();
    List<Notification> GetNotificationsBy(EnumNotificationType notificationType);
    List<Notification> GetNotificationsBy(EnumNotificationType notificationType, string messageKey);

    void SetNotification(Notification notification);
    void SetNotification(EnumNotificationType notificationType, string message);
    void SetNotification(EnumNotificationType notificationType, string message, string messageKey);
}