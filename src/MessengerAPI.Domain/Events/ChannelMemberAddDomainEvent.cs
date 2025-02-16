using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Events;

public record ChannelMemberAddDomainEvent : ChannelMemberDomainEvent
{
    public ChannelMemberAddDomainEvent(Channel Channel, ChannelMemberInfo MemberInfo, long InitiatorId)
        : base(Channel, MemberInfo, InitiatorId) { }
}
