using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Channels.Events;

public class MessageUpdateGatewayEvent : GatewayEventDto
{
    public IReadOnlyList<string> Recipients { get; init; }
    public MessageSchema Payload { get; init; }
    public MessageExtra Extra { get; init; }

    public MessageUpdateGatewayEvent(
        MessageSchema message,
        IEnumerable<string> recipients,
        ChannelType channelType)
        : base("MESSAGE_UPDATE")
    {
        Recipients = recipients.ToList();
        Payload = message;
        Extra = new MessageExtra
        {
            ChannelType = channelType,
        };
    }
}
