using Cassandra;
using Messenger.Domain.Entities;

namespace Messenger.Data.Scylla.Messages.Queries;

public class MessageAckQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByMessageId;

    public MessageAckQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO message_acks (
                channelid,
                userid,
                lastreadmessageid,
                timestamp
            ) VALUES (?, ?, ?, ?)
            USING TTL ?;
        """);

        _selectByMessageId = session.Prepare("""
            SELECT *
            FROM message_acks
            WHERE channelid = ?
                AND lastreadmessageid >= ?
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
