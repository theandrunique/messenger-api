using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageCreateDomainEvent(
    Channel Channel,
    Message Message,
    User Initiator) : IDomainEvent;
