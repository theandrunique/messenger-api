using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.ChannelAggregate.Events;

/// <summary>
/// Event when a message is updated
/// </summary>
/// <param name="NewMessage"><see cref="Message"/></param>
public record MessageUpdated(Message NewMessage) : IDomainEvent;
