using MediatR;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record UserCreated(User User) : INotification;
