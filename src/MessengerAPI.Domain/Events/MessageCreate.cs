using MediatR;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Models.Events;

public record MessageCreate(Message Message) : INotification;
