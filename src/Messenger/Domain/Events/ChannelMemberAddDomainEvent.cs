using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Domain.Events;

public record ChannelMemberAddDomainEvent(
    Channel Channel,
    ChannelMemberInfo MemberInfo,
    long InitiatorId) : IDomainEvent;
