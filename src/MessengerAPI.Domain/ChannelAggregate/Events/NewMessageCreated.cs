using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common;

namespace MessengerAPI.Domain.ChannelAggregate.Events;

/// <summary>
/// Event when a new message is created
/// </summary>
/// <param name="NewMessage"><see cref="Message"/></param>
public record NewMessageCreated(Message NewMessage) : IDomainEvent;
