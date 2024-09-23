using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.ChannelAggregate.Events;

/// <summary>
/// Event when a new channel is created
/// </summary>
/// <param name="NewChannel"><see cref="Channel"/></param>
public record NewChannelCreated(Channel NewChannel) : IDomainEvent;
