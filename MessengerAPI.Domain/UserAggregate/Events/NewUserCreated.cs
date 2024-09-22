using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.UserAggregate.Events;

public record NewUserCreated(User NewUser) : IDomainEvent;
