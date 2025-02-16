using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageAckDomainEvent(Message Message, Channel Channel) : IDomainEvent;