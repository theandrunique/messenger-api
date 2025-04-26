using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Events;

public record MessageUpdateDomainEvent(
    Channel Channel,
    Message Message,
    ChannelMemberInfo Initiator) : IDomainEvent;
