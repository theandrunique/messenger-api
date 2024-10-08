using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Infrastructure.Common.WebSockets;

public interface IUpdatesClient
{
    Task NewMessageReceived(MessageSchema message);
    Task MessageUpdated(MessageSchema message);
    Task NewChannelCreated(ChannelSchema channel);
}

