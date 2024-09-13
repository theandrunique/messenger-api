using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;

namespace MessengerAPI.Application.Schemas.Notifications;

public record NewMessageNotificationSchema
{
    public NotificationType Type => NotificationType.NewMessage;
    public MessageSchema Data { get; init; }
    // public NewMessageNotificationSchema(Message message) => Data = message;
}
