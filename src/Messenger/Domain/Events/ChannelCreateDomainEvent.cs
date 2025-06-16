using Messenger.Domain.Channels;

namespace Messenger.Domain.Events;

public record ChannelCreateDomainEvent(Channel Channel) : IDomainEvent;
