using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Schemas.Notifications;

public record NewMessageNotificationSchema
{
    public NotificationType Type => NotificationType.NewMessage;
    public MessageSchema Data { get; init; }
}
