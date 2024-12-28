using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.Events;

public record NewMessageCreated(Message NewMessage) : IDomainEvent;
