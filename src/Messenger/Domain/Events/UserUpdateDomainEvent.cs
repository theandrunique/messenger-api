using Messenger.Domain.Entities;

namespace Messenger.Domain.Events;

public record UserUpdateDomainEvent(User User) : IDomainEvent;
