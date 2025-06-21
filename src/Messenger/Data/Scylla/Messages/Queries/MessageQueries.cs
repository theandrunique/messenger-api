using Cassandra;
using Messenger.Data.Scylla.Common;
using Messenger.Domain.Messages;

namespace Messenger.Data.Scylla.Messages.Queries;

public class MessageQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByIds;
    private readonly PreparedStatement _selectByChannelId;
    private readonly PreparedStatement _deleteById;

    public MessageQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO messages (
                channel_id,
                message_id,
                author_id,
                target_user_id,
                content,
                timestamp,
                edited_timestamp,
                pinned,
                referenced_message_id,
                type,
                metadata
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
            """);

        _selectById = session.Prepare("SELECT * FROM messages WHERE channel_id = ? AND message_id = ?");

        _selectByIds = session.Prepare("SELECT * FROM messages WHERE channel_id = ? AND message_id IN ?");

        _selectByChannelId = session.Prepare("SELECT * FROM messages WHERE channel_id = ? AND message_id < ? ORDER BY message_id DESC LIMIT ?");

        _deleteById = session.Prepare("DELETE FROM messages WHERE channel_id = ? AND message_id = ?");
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
            message.ReferencedMessageId,
            (int)message.Type,
            MessageMetadataConverter.ToJson(message.Metadata));
    }

    public BoundStatement SelectById(long channelId, long messageId)
    {
        return _selectById.Bind(channelId, messageId);
    }

    public BoundStatement SelectByIds(long channelId, IEnumerable<long> messageIds)
    {
        return _selectByIds.Bind(channelId, messageIds);
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
