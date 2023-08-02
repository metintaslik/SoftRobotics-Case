using MassTransit;
using softrobotics.communication.api.Common;
using softrobotics.communication.api.Interfaces;
using softrobotics.communication.api.Services;
using softrobotics.shared.Models;

namespace softrobotics.communication.api.Worker;

public class MailConsumer : IConsumer<UserCreatedEventModel>
{
    private readonly INotificationProvider mailProvider;

    public MailConsumer()
    {
        mailProvider = new NotificationProviderFactory(NotificationType.Mail).CreateNotificationProvider();
    }

    public Task Consume(ConsumeContext<UserCreatedEventModel> context)
    {
        mailProvider.SendNotification($"Mailto: {context.Message.Mail} - UserID: {context.Message.UserID}");
        return Task.CompletedTask;
    }
}