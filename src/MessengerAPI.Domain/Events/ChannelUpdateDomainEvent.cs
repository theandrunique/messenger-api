using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record ChannelUpdateDomainEvent(Channel Channel) : IDomainEvent;
