using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Core.Notification;
using FluentValidation.Results;

namespace EcommercePortfolio.Core.Messaging;

public abstract class CommandHandler(INotificationContext notification)
{
    protected readonly INotificationContext _notification = notification;

    protected void AddError(string message, EnumNotificationType? notificationType = null)
    {
        _notification.Add(notificationType ?? EnumNotificationType.VALIDATION_ERROR, message);
    }

    protected void AddError(ValidationResult validationResult)
    {
        _notification.Add(EnumNotificationType.VALIDATION_ERROR, validationResult);
    }

    protected async Task PersistData(IUnitOfWork unityOfWork)
    {
        if (!await unityOfWork.Commit())
            _notification.Add(
                EnumNotificationType.DATABASE_ERROR,
                "An error occurred while trying to persist data");
    }
}
