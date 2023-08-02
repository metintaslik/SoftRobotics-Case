using softrobotics.auth.application.Common.Model;
using softrobotics.shared.Models;

namespace softrobotics.auth.application.UserHandler.Events;

public class UserCreatedEvent : BaseEvent
{
    public UserCreatedEvent(UserCreatedEventModel model)
    {
        UserCreatedEventModel = model;
    }

    public UserCreatedEventModel UserCreatedEventModel { get; set; }
}