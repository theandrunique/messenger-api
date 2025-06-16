using Messenger.Domain.Channels;

namespace Messenger.Domain.Events;

public class MessageDeleteDomainEvent : IDomainEvent
{
    public Channel Channel { get; init; }
    public long MessageId { get; init; }
    public long InitiatorId { get; init; }
    public Message? NewLastMessage { get; init; }

    public MessageDeleteDomainEvent(Channel channel, long messageId, Message? newLastMessage, long initiatorId)
    {
        Channel = channel;
        MessageId = messageId;
        NewLastMessage = newLastMessage;
        InitiatorId = initiatorId;
    }
}
