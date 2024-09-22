using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Schemas.Notifications;

public record NewChannelNotificationSchema
{
    public NotificationType Type => NotificationType.ChannelCreated;
    public ChannelSchema Data { get; init; }
}
