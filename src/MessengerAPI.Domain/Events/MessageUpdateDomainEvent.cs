using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageUpdateDomainEvent(
    Channel Channel,
    Message Message,
    User Initiator) : IDomainEvent;
