using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Common.Interfaces;

public interface INotificationService
{
    Task NewMessageReceived(List<Guid> recipientIds, MessageSchema message);
    Task MessageUpdated(List<Guid> recipientIds, MessageSchema message);
    Task NewChannelCreated(List<Guid> recipientIds, ChannelSchema channel);
}

