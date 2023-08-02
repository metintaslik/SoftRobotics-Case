using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using softrobotics.shared.Models;

namespace softrobotics.auth.application.UserHandler.Events;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User created event handled. {nameof(UserCreatedEvent)}");
        publishEndpoint.Publish<UserCreatedEventModel>(notification.UserCreatedEventModel, cancellationToken);

        return Task.CompletedTask;
    }
}