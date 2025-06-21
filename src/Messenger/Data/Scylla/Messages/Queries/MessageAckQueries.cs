using Cassandra;
using Messenger.Domain.Channels;

namespace Messenger.Data.Scylla.Messages.Queries;

public class MessageAckQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByMessageId;

    public MessageAckQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO message_acks (
                channel_id,
                user_id,
                last_read_message_id,
                timestamp
            ) VALUES (?, ?, ?, ?)
            USING TTL ?;
        """);

        _selectByMessageId = session.Prepare("""
            SELECT *
            FROM message_acks
            WHERE channel_id = ?
                AND last_read_message_id >= ?
        """);
    }

    public BoundStatement Insert(MessageAck messageAck)
    {
        return _insert.Bind(
            messageAck.ChannelId,
            messageAck.UserId,
            messageAck.LastReadMessageId,
            messageAck.Timestamp,
            MessageAck.ExpireSeconds);
    }

    public BoundStatement SelectByMessageId(long channelId, long lastReadMessageId)
    {
        return _selectByMessageId.Bind(channelId, lastReadMessageId);
    }
}
