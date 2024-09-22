using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.ChannelAggregate.Events;

public record NewChannelCreated(Channel NewChannel) : IDomainEvent;
