using Messenger.Domain.Auth;

namespace Messenger.Domain.Events;

public record UserUpdateDomainEvent(User User) : IDomainEvent;
