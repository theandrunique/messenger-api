using MediatR;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record ChannelCreate(Channel Channel) : INotification;
