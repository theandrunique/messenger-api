using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Schemas.Notifications;

/// <summary>
/// Message updated notification schema
/// </summary>
public record MessageUpdatedNotificationSchema : INotificationSchema<MessageSchema>
{
    public NotificationType Type => NotificationType.MessageUpdated;
    public MessageSchema Data { get; init; }
}
