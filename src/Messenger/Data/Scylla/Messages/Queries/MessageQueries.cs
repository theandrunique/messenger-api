using Cassandra;
using Messenger.Data.Scylla.Common;
using Messenger.Domain.Entities;
using Newtonsoft.Json;

namespace Messenger.Data.Scylla.Messages.Queries;

public class MessageQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByChannelId;
    private readonly PreparedStatement _deleteById;

    public MessageQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO messages (
                channelid,
                id,
                authorid,
                targetuserid,
                content,
                timestamp,
                editedtimestamp,
                pinned,
                type,
                metadata) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
            """);

        _selectById = session.Prepare("SELECT * FROM messages WHERE channelid = ? AND id = ?");

        _selectByChannelId = session.Prepare("SELECT * FROM messages WHERE channelid = ? AND id < ? ORDER BY id DESC LIMIT ?");

        _deleteById = session.Prepare("DELETE FROM messages WHERE channelid = ? AND id = ?");
    }

    public BoundStatement Insert(Message message)
    {
        return _insert.Bind(
            message.ChannelId,
            message.Id,
            message.AuthorId,
            message.TargetUserId,
            message.Content,
            message.Timestamp,
            message.EditedTimestamp,
            message.Pinned,
            (int)message.Type,
            MessageMetadataConverter.ToJson(message.Metadata));
    }

    public BoundStatement SelectById(long channelId, long messageId)
    {
        return _selectById.Bind(channelId, messageId);
    }

    public BoundStatement SelectByChannelId(long channelId, long before, int limit)
    {
        return _selectByChannelId.Bind(channelId, before, limit);
    }

    public BoundStatement DeleteById(long channelId, long messageId)
    {
        return _deleteById.Bind(channelId, messageId);
    }
}
