using Cassandra;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Queries;

public class ChannelUserQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByChannelId;
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
                userid,
                channelid,
                lastreadmessageid,
                username,
                globalname,
                image,
                permissionoverwrites,
                isleave
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
            """);

        _selectByChannelId = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channelid = ?");

        _selectByChannelIds = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channelid IN ?");

        _selectByUserId = session.Prepare("SELECT * FROM channel_users_by_user_id WHERE userid = ?");

        _selectByChannelIdAndUserIds = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channelid = ? AND userid IN ?");

        _selectChannelIdsByUserId = session.Prepare("SELECT channelid FROM channel_users_by_user_id WHERE userid = ?");

        _updateLastReadMessageId = session.Prepare("UPDATE channel_users_by_user_id SET lastreadmessageid = ? WHERE userid = ? AND channelid = ?");

        _updateUserInfo = session.Prepare("UPDATE channel_users_by_user_id SET globalname = ?, image = ? WHERE userid = ? AND channelid IN ?");

        _updateUsername = session.Prepare("UPDATE channel_users_by_user_id SET username = ? WHERE userid = ? AND channelid IN ?");

        _updateIsLeave = session.Prepare("UPDATE channel_users_by_user_id SET isleave = ? WHERE userid = ? AND channelid = ?");
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
