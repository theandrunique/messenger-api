using Messenger.Domain.Entities;

namespace Messenger.Domain.Events;

public record ChannelNameUpdateDomainEvent(
    Channel Channel,
    string NewName,
    long InitiatorId) : IDomainEvent;
