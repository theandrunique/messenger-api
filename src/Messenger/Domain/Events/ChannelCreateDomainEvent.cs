using Messenger.Domain.Entities;

namespace Messenger.Domain.Events;

public record ChannelCreateDomainEvent(Channel Channel) : IDomainEvent;
