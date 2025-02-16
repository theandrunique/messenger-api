using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Events;

public record ChannelMemberAddDomainEvent(
    Channel Channel,
    ChannelMemberInfo MemberInfo,
    long InitiatorId) : IDomainEvent;
