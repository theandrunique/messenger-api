using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageUpdateDomainEvent : MessageDomainEvent
{
    public MessageUpdateDomainEvent(Channel Channel, Message Message, User Initiator)
        : base(Channel, Message, Initiator) { }
}
