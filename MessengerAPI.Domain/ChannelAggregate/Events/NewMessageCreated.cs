using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.ChannelAggregate.Events;

public record NewMessageCreated(
    Message NewMessage,
    Channel Channel
) : IDomainEvent;
