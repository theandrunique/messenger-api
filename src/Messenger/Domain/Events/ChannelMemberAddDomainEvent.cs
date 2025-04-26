using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Events;

public record ChannelMemberAddDomainEvent(
    Channel Channel,
    ChannelMemberInfo MemberInfo,
    long InitiatorId) : IDomainEvent;
