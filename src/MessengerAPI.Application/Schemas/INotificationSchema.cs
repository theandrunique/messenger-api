using MessengerAPI.Application.Common;

namespace MessengerAPI.Application.Schemas;

public interface INotificationSchema 
{
    public NotificationType Type { get; }
    public object Data { get; }
}
