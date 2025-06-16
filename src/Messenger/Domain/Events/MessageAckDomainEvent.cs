using Messenger.Domain.Channels;

namespace Messenger.Domain.Events;

public record MessageAckDomainEvent(long messageId, Channel Channel, long initiatorId) : IDomainEvent;
