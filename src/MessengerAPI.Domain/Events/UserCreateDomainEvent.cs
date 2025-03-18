using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record UserCreateDomainEvent(User User) : IDomainEvent;
