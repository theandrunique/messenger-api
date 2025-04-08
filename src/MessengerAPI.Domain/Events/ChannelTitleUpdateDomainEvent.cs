using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record ChannelNameUpdateDomainEvent(
    Channel Channel,
    string NewName,
    long InitiatorId) : IDomainEvent;
