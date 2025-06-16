using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Domain.Events;

public record MessageUpdateDomainEvent(
    Channel Channel,
    Message Message,
    ChannelMemberInfo Initiator) : IDomainEvent;
