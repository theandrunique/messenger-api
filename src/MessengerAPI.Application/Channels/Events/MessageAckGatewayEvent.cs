using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Events;

public class MessageAckGatewayEvent : GatewayEventDto
{
    public string ChannelId { get; init; }
    public string MessageId { get; init; }
    public string MemberId { get; init; }

    public MessageAckGatewayEvent(
        long channelId,
        long messageId,
        long memberId,
        IEnumerable<string> recipients)
        : base(GatewayEventType.MESSAGE_ACK, recipients)
    {
        ChannelId = channelId.ToString();
        MessageId = messageId.ToString();
        MemberId = memberId.ToString();
    }
}
