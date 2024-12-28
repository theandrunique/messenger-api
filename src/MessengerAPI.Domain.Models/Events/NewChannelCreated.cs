using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.Events;

public record NewChannelCreated(Channel NewChannel) : IDomainEvent;

