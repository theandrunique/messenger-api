using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageAckDomainEvent(long messageId, Channel Channel) : IDomainEvent;