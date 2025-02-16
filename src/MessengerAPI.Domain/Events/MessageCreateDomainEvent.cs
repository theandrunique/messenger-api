using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.Events;

public record MessageCreateDomainEvent : MessageDomainEvent
{
    public MessageCreateDomainEvent(Channel Channel, Message Message, User Initiator)
        : base(Channel, Message, Initiator) { }
}
