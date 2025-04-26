using Cassandra;
using MessengerAPI.Data.Implementations.Channels.Dto;
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
        _insert = session.Prepare("INSERT INTO channels_by_id (channelid, channeltype, name, ownerid, image, permissionoverwrites) VALUES (?, ?, ?, ?, ?, ?)");

        _selectById = session.Prepare("SELECT * FROM channels_by_id WHERE channelid = ?");

        _selectByIds = session.Prepare("SELECT * FROM channels_by_id WHERE channelid IN ?");

        _updateChannelInfo = session.Prepare("UPDATE channels_by_id SET name = ?, image = ? WHERE channelid = ?");

        _updateOwnerId = session.Prepare("UPDATE channels_by_id SET ownerid = ? WHERE channelid = ?");

        _updateLastMessageInfo = session.Prepare("UPDATE channels_by_id SET lastmessagetimestamp = ?, lastmessage = ? WHERE channelid = ?");
    }

    public BoundStatement Insert(Channel channel)
    {
        return _insert.Bind(
            channel.Id,
            (int)channel.Type,
            channel.Name,
            channel.OwnerId,
            channel.Image,
            channel.PermissionOverwrites.HasValue
                ? channel.PermissionOverwrites.Value.ToValue()
                : null);
    }

    public BoundStatement SelectById(long channelId)
    {
        return _selectById.Bind(channelId);
    }

    public BoundStatement SelectByIds(IEnumerable<long> channelIds)
    {
        return _selectByIds.Bind(channelIds);
    }

    public BoundStatement UpdateChannelInfo(long channelId, string name, string? image)
    {
        return _updateChannelInfo.Bind(name, image, channelId);
    }

    public BoundStatement UpdateOwnerId(long channelId, long ownerId)
    {
        return _updateOwnerId.Bind(ownerId, channelId);
    }

    public BoundStatement UpdateLastMessageInfo(Message message)
    {
        var messageInfo = new MessageInfo(message);

        return _updateLastMessageInfo.Bind(
            messageInfo.Timestamp,
            MessageInfoDto.From(messageInfo),
            message.ChannelId);
    }
}
