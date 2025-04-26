using Messenger.Domain.Entities;

namespace Messenger.Domain.Events;

public record UserCreateDomainEvent(User User) : IDomainEvent;
