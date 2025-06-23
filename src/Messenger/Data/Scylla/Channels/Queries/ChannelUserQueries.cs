using Cassandra;
using Messenger.Domain.Auth;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Queries;

public class ChannelUserQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByChannelId;
    private readonly PreparedStatement _selectMemberIdsByChannelId;
    private readonly PreparedStatement _selectByChannelIds;
    private readonly PreparedStatement _selectByUserId;
    private readonly PreparedStatement _selectByChannelIdAndUserIds;
    private readonly PreparedStatement _selectChannelIdsByUserId;
    private readonly PreparedStatement _updateLastReadMessageId;
    private readonly PreparedStatement _updateUserInfo;
    private readonly PreparedStatement _updateUsername;
    private readonly PreparedStatement _updateIsLeave;

    public ChannelUserQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO channel_users_by_user_id (
                user_id,
                channel_id,
                last_read_message_id,
                username,
                global_name,
                image,
                permission_overwrites,
                is_leave
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
            """);

        _selectByChannelId = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channel_id = ?");

        _selectMemberIdsByChannelId = session.Prepare("SELECT user_id, is_leave FROM channel_users_by_channel_id WHERE channel_id = ?");

        _selectByChannelIds = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channel_id IN ?");

        _selectByUserId = session.Prepare("SELECT * FROM channel_users_by_user_id WHERE user_id = ?");

        _selectByChannelIdAndUserIds = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channel_id = ? AND user_id IN ?");

        _selectChannelIdsByUserId = session.Prepare("SELECT channel_id FROM channel_users_by_user_id WHERE user_id = ?");

        _updateLastReadMessageId = session.Prepare("UPDATE channel_users_by_user_id SET last_read_message_id = ? WHERE user_id = ? AND channel_id = ?");

        _updateUserInfo = session.Prepare("UPDATE channel_users_by_user_id SET global_name = ?, image = ? WHERE user_id = ? AND channel_id IN ?");

        _updateUsername = session.Prepare("UPDATE channel_users_by_user_id SET username = ? WHERE user_id = ? AND channel_id IN ?");

        _updateIsLeave = session.Prepare("UPDATE channel_users_by_user_id SET is_leave = ? WHERE user_id = ? AND channel_id = ?");
    }

    public BoundStatement Insert(long channelId, ChannelMemberInfo member)
    {
        return _insert.Bind(
            member.UserId,
            channelId,
            member.LastReadMessageId,
            member.Username,
            member.GlobalName,
            member.Image,
            member.PermissionOverwrites.HasValue
                ? (long)member.PermissionOverwrites.Value.ToValue()
                : null,
            member.IsLeave);
    }

    public BoundStatement SelectByChannelId(long channelId)
    {
        return _selectByChannelId.Bind(channelId);
    }

    public BoundStatement SelectMemberIdsByChannelId(long channelId)
    {
        return _selectMemberIdsByChannelId.Bind(channelId);
    }

    public BoundStatement SelectByChannelIds(IEnumerable<long> channelIds)
    {
        return _selectByChannelIds.Bind(channelIds);
    }

    public BoundStatement SelectByUserId(long userId)
    {
        return _selectByUserId.Bind(userId);
    }

    public BoundStatement SelectByChannelIdAndUserIds(long channelId, IEnumerable<long> userIds)
    {
        return _selectByChannelIdAndUserIds.Bind(channelId, userIds);
    }

    public BoundStatement SelectChannelIdsByUserId(long userId)
    {
        return _selectChannelIdsByUserId.Bind(userId);
    }

    public BoundStatement UpdateLastReadMessageId(long userId, long channelId, long lastReadMessageId)
    {
        return _updateLastReadMessageId.Bind(lastReadMessageId, userId, channelId);
    }

    public BoundStatement UpdateUserInfo(User user, IEnumerable<long> channelIds)
    {
        return _updateUserInfo.Bind(user.GlobalName, user.Image, user.Id, channelIds);
    }

    public BoundStatement UpdateUsername(string username, IEnumerable<long> channelIds)
    {
        return _updateUsername.Bind(username, channelIds);
    }

    public BoundStatement UpdateIsLeave(long userId, long channelId, bool isLeave)
    {
        return _updateIsLeave.Bind(isLeave, userId, channelId);
    }
}
