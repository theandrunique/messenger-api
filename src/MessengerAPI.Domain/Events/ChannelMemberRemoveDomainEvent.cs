using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Events;

public record ChannelMemberRemoveDomainEvent : ChannelMemberDomainEvent
{
    public ChannelMemberRemoveDomainEvent(Channel Channel, ChannelMemberInfo MemberInfo, long InitiatorId)
        : base(Channel, MemberInfo, InitiatorId) { }
}
