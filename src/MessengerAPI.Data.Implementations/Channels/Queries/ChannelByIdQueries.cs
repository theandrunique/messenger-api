using Cassandra;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Implementations.Channels.Queries;

public class ChannelByIdQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByIds;
    private readonly PreparedStatement _updateChannelInfo;
    private readonly PreparedStatement _updateOwnerId;
    private readonly PreparedStatement _updateLastMessageInfo;

    public ChannelByIdQueries(ISession session)
    {
        _insert = session.Prepare("INSERT INTO channels_by_id (channelid, channeltype, title, ownerid, image) VALUES (?, ?, ?, ?, ?)");

        _selectById = session.Prepare("SELECT * FROM channels_by_id WHERE channelid = ?");

        _selectByIds = session.Prepare("SELECT * FROM channels_by_id WHERE channelid IN ?");

        _updateChannelInfo = session.Prepare("UPDATE channels_by_id SET title = ?, image = ? WHERE channelid = ?");

        _updateOwnerId = session.Prepare("UPDATE channels_by_id SET ownerid = ? WHERE channelid = ?");

        _updateLastMessageInfo = session.Prepare("UPDATE channels_by_id SET lastmessagetimestamp = ?, lastmessage = ? WHERE channelid = ?");
    }

    public BoundStatement Insert(Channel channel)
    {
        return _insert.Bind(channel.Id, (int)channel.Type, channel.Title, channel.OwnerId, channel.Image);
    }

    public BoundStatement SelectById(long channelId)
    {
        return _selectById.Bind(channelId);
    }

    public BoundStatement SelectByIds(IEnumerable<long> channelIds)
    {
        return _selectByIds.Bind(channelIds);
    }

    public BoundStatement UpdateChannelInfo(long channelId, string title, string? image)
    {
        return _updateChannelInfo.Bind(title, image, channelId);
    }

    public BoundStatement UpdateOwnerId(long channelId, long ownerId)
    {
        return _updateOwnerId.Bind(ownerId, channelId);
    }

    public BoundStatement UpdateLastMessageInfo(Message message)
    {
        var messageInfo = new MessageInfo(message);
        return _updateLastMessageInfo.Bind(messageInfo.Timestamp, messageInfo, message.ChannelId);
    }
}
