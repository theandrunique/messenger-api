using Cassandra;
using Messenger.Data.Scylla.Channels.Dto;
using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Messages;

namespace Messenger.Data.Scylla.Channels.Queries;

public class ChannelByIdQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByIds;
    private readonly PreparedStatement _updateName;
    private readonly PreparedStatement _updateImage;
    private readonly PreparedStatement _updateOwnerId;
    private readonly PreparedStatement _updateLastMessageInfo;

    public ChannelByIdQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO channels_by_id (
                channel_id,
                type,
                name,
                owner_id,
                image,
                permission_overwrites
            ) VALUES (?, ?, ?, ?, ?, ?)
            """);

        _selectById = session.Prepare("SELECT * FROM channels_by_id WHERE channel_id = ?");

        _selectByIds = session.Prepare("SELECT * FROM channels_by_id WHERE channel_id IN ?");

        _updateName = session.Prepare("UPDATE channels_by_id SET name = ? WHERE channel_id = ?");

        _updateImage = session.Prepare("UPDATE channels_by_id SET image = ? WHERE channel_id = ?");

        _updateOwnerId = session.Prepare("UPDATE channels_by_id SET owner_id = ? WHERE channel_id = ?");

        _updateLastMessageInfo = session.Prepare("UPDATE channels_by_id SET last_message_timestamp = ?, last_message = ? WHERE channel_id = ?");
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

    public BoundStatement UpdateName(long channelId, string name)
    {
        return _updateName.Bind(name, channelId);
    }

    public BoundStatement UpdateImage(long channelId, string image)
    {
        return _updateImage.Bind(image, channelId);
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

    public BoundStatement UpdateLastMessageInfo(long channelId, MessageInfo? message)
    {
        if (message is null)
        {
            return _updateLastMessageInfo.Bind(null, null, channelId);
        }
        else
        {
            return _updateLastMessageInfo.Bind(
                message.Value.Timestamp,
                MessageInfoDto.From(message.Value),
                channelId);
        }
    }
}
