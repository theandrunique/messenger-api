using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Events;

public class ChannelUpdateDomainEvent : IDomainEvent
{
    public Channel Channel { get; init; }
    public long InitiatorId { get; init; }
    public string? NewImage { get; set; }
    public string? NewName { get; set; }
    public MessageInfo? NewLastMessage { get; set; }

    public ChannelUpdateDomainEvent(Channel channel, long initiatorId)
    {
        Channel = channel;
        InitiatorId = initiatorId;
    }
}
