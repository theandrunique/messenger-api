using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record ChannelTitleUpdateDomainEvent(
    Channel Channel,
    string NewTitle,
    long InitiatorId) : IDomainEvent;
