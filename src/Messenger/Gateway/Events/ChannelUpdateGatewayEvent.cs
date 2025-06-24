using System.Text.Json.Serialization;
using Messenger.Gateway.Common;

namespace Messenger.Gateway.Events;

public class ChannelUpdateGatewayEvent : IGatewayEvent
{
    [JsonIgnore]
    public GatewayEventType EventType => GatewayEventType.CHANNEL_UPDATE;

    public string ChannelId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Image { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    public ChannelUpdateGatewayEvent(long channelId)
    {
        ChannelId = channelId.ToString();
    }
}
