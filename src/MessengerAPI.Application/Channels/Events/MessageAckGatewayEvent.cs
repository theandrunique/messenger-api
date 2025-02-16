using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class MessageAckGatewayEvent : GatewayEventDto
{
    public string ChannelId { get; init; }
    public string MessageId { get; init; }

    public MessageAckGatewayEvent(
        long channelId,
        long messageId,
        IEnumerable<string> recipients)
        : base(GatewayEventType.MESSAGE_ACK, recipients)
    {
        ChannelId = channelId.ToString();
        MessageId = messageId.ToString();
    }
}
