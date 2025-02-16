using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Channels.Events;

public class MessageCreateGatewayEvent : GatewayEventDto
{
    public MessageSchema Payload { get; init; }
    public MessageExtra Extra { get; init; }

    public MessageCreateGatewayEvent(
        MessageSchema message,
        IEnumerable<string> recipients,
        ChannelType channelType)
        : base(GatewayEventType.MESSAGE_CREATE, recipients)
    {
        Payload = message;
        Extra = new MessageExtra
        {
            ChannelType = channelType,
        };
    }
}
