using softrobotics.communication.api.Common;
using softrobotics.communication.api.Interfaces;

namespace softrobotics.communication.api.Services;

public class NotificationProviderFactory : INotificationProviderFactory
{
    private readonly NotificationType notificationType;

    public NotificationProviderFactory(NotificationType notificationType)
    {
        this.notificationType = notificationType;
    }

    public INotificationProvider CreateNotificationProvider()
    {
        return notificationType switch
        {
            NotificationType.Sms or NotificationType.AppNotification or NotificationType.Mail => new MailNotificationProvider(),
            _ => throw new NotSupportedException("Unsupported notification type."),
        };
    }
}
