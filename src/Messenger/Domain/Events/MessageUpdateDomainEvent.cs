using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Messages;

namespace Messenger.Domain.Events;

public record MessageUpdateDomainEvent(
    Channel Channel,
    Message Message,
    ChannelMemberInfo Initiator) : IDomainEvent;
