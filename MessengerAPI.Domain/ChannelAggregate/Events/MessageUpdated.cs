using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.ChannelAggregate.Events;

public record MessageUpdated(Message NewMessage) : IDomainEvent;
