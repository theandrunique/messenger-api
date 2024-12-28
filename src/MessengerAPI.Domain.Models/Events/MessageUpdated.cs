using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.Events;

public record MessageUpdated(Message NewMessage) : IDomainEvent;

