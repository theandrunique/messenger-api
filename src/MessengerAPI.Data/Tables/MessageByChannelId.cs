using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Tables;

internal class MessageByChannelId
{
    public long Id { get; set; }
    public long ChannelId { get; set; }
    public long SenderId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? ReplyTo { get; set; }
    public MessageSenderInfo Author { get; set; }
    public List<Attachment> Attachments { get; set; }

    public static MessageByChannelId FromMessage(Message message)
    {
        return new MessageByChannelId()
        {
            Id = message.Id,
            ChannelId = message.ChannelId,
            SenderId = message.SenderId,
            Content = message.Content,
            SentAt = message.SentAt,
            UpdatedAt = message.UpdatedAt,
            ReplyTo = message.ReplyTo,
            Author = message.Author,
            Attachments = message.Attachments
        };
    }

    public Message ToMessage()
    {
        return new Message(
            ChannelId,
            Id,
            Content,
            SentAt,
            UpdatedAt,
            ReplyTo,
            Author,
            Attachments);
    }
}
