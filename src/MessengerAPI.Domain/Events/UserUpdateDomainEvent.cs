using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record UserUpdateDomainEvent(User User) : IDomainEvent;
