namespace MessengerAPI.Domain.Entities;

public class MessageAck
{
    public long ChannelId { get; private set; }
    public long UserId { get; private set; }
    public long LastReadMessageId { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

    public readonly static int ExpireSeconds = TimeSpan.FromDays(7).Seconds;

    public MessageAck(long channelId, long userId, long lastReadMessageId)
    {
        ChannelId = channelId;
        UserId = userId;
        LastReadMessageId = lastReadMessageId;
        Timestamp = DateTimeOffset.UtcNow;
    }

    public MessageAck(long channelId, long userId, long lastReadMessageId, DateTimeOffset timestamp)
    {
        ChannelId = channelId;
        UserId = userId;
        LastReadMessageId = lastReadMessageId;
        Timestamp = timestamp;
    }
}
