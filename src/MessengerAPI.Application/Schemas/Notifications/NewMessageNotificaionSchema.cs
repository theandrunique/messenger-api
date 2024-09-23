using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Schemas.Notifications;

/// <summary>
/// New message notification schema
/// </summary>
public record NewMessageNotificationSchema : INotificationSchema<MessageSchema>
{
    public NotificationType Type => NotificationType.NewMessage;
    public MessageSchema Data { get; init; }
}
