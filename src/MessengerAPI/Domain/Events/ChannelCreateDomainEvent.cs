using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record ChannelCreateDomainEvent(Channel Channel) : IDomainEvent;
