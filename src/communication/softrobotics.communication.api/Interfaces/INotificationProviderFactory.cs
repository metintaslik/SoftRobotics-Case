using softrobotics.communication.api.Common;

namespace softrobotics.communication.api.Interfaces;

public interface INotificationProviderFactory
{
    INotificationProvider CreateNotificationProvider();
}