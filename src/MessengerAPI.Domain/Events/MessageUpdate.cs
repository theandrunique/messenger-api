using MediatR;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageUpdate(Message Message) : INotification;
