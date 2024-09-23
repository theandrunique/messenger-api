using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.UserAggregate.Events;

/// <summary>
/// Event when a new user is created
/// </summary>
/// <param name="NewUser"><see cref="User"/></param>
public record NewUserCreated(User NewUser) : IDomainEvent;
