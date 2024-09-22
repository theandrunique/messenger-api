using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Schemas.Notifications;

public record MessageUpdatedNotificationSchema
{
    public NotificationType Type => NotificationType.MessageUpdated;
    public MessageSchema Data { get; init; }
}
