using MessengerAPI.Domain.Models;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.UserAggregate.Events;

public record NewUserCreated(User NewUser) : IDomainEvent;

