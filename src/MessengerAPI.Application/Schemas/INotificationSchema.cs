using MessengerAPI.Application.Common;

namespace MessengerAPI.Application.Schemas;

public interface INotificationSchema<T>
{
    /// <summary>
    /// Notification type
    /// </summary>
    NotificationType Type { get; }
    /// <summary>
    /// Notification data
    /// </summary>
    T Data { get; }
}
