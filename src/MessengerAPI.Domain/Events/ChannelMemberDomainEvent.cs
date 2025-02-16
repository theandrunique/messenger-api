using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Events;

public abstract record ChannelMemberDomainEvent(
    Channel Channel,
    ChannelMemberInfo MemberInfo,
    long InitiatorId) : IDomainEvent;
