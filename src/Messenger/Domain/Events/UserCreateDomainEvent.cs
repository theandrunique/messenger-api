using Messenger.Domain.Auth;

namespace Messenger.Domain.Events;

public record UserCreateDomainEvent(User User) : IDomainEvent;
