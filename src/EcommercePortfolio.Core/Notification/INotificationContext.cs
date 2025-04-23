using FluentValidation.Results;
using MediatR;

namespace EcommercePortfolio.Core.Notification;

public interface INotificationContext
{
    bool Any();
    bool Any(EnumNotificationType notificationType);
    bool Any(EnumNotificationType notificationType, string messageKey);
    bool AnyExcept(EnumNotificationType notificationType);
    bool AnyExcept(IReadOnlyCollection<EnumNotificationType> notificationsType);

    IReadOnlyCollection<Notification> Get();
    IReadOnlyCollection<Notification> Get(EnumNotificationType notificationType);
    IReadOnlyCollection<Notification> Get(EnumNotificationType notificationType, string messageKey);
    IReadOnlyCollection<Notification> GetAllExcept(EnumNotificationType notificationType);
    IReadOnlyCollection<Notification> GetAllExcept(IReadOnlyCollection<EnumNotificationType> notificationsType);
    IReadOnlyCollection<string> GetOnlyMessages();
    IReadOnlyCollection<string> GetOnlyMessages(EnumNotificationType notificationType);
    IReadOnlyCollection<string> GetOnlyMessages(EnumNotificationType notificationType, string messageKey);

    void Add(Notification notification);
    void Add(EnumNotificationType notificationType, string message);
    void Add(EnumNotificationType notificationType, string message, string messageKey);
    void Add(EnumNotificationType notificationType, ValidationResult validationResult);
}