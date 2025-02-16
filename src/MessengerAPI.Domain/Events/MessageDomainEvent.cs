using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public abstract record MessageDomainEvent(
    Channel Channel,
    Message Message,
    User Initiator) : IDomainEvent;
