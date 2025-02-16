using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Events;

public record ChannelMemberRemoveDomainEvent(
    Channel Channel,
    ChannelMemberInfo MemberInfo,
    long InitiatorId) : IDomainEvent;
