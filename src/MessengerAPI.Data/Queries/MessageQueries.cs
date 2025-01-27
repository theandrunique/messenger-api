using Cassandra;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Queries;

internal class MessageQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByChannelId;

    public MessageQueries(ISession session)
    {
        _insert = session.Prepare("INSERT INTO messages (channelid, id, authorid, content, timestamp, editedtimestamp, pinned, type) VALUES (?, ?, ?, ?, ?, ?, ?, ?)");
        _selectById = session.Prepare("SELECT * FROM messages WHERE channelid = ? AND id = ?");
        _selectByChannelId = session.Prepare("SELECT * FROM messages WHERE channelid = ? AND id < ? ORDER BY id DESC LIMIT ?");
    }

    public BoundStatement Insert(Message message)
    {
        return _insert.Bind(message.ChannelId, message.Id, message.AuthorId, message.Content, message.Timestamp, message.EditedTimestamp, message.Pinned, (int)message.Type);
    }

    public BoundStatement SelectById(long channelId, long messageId)
    {
        return _selectById.Bind(channelId, messageId);
    }

    public BoundStatement SelectByChannelId(long channelId, long before, int limit)
    {
        return _selectByChannelId.Bind(channelId, before, limit);
    }

}
