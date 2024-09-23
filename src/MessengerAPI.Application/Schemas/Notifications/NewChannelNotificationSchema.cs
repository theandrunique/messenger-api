using MessengerAPI.Application.Common;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Schemas.Notifications;

/// <summary>
/// New channel notification schema
/// </summary>
public record NewChannelNotificationSchema : INotificationSchema<ChannelSchema>
{
    public NotificationType Type => NotificationType.ChannelCreated;
    public ChannelSchema Data { get; init; }
}
