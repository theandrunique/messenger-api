using Cassandra;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Queries;

internal class ChannelUserQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _delete;
    private readonly PreparedStatement _selectByChannelId;
    private readonly PreparedStatement _selectByChannelIds;
    private readonly PreparedStatement _selectByUserId;
    private readonly PreparedStatement _selectByChannelIdAndUserIds;
    private readonly PreparedStatement _updateReadAt;

    public ChannelUserQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO channel_users_by_user_id (
                userid,
                channelid,
                readat,
                username,
                globalname,
                image,
                permissions
            ) VALUES (?, ?, ?, ?, ?, ?, ?)
            """);

        _delete = session.Prepare("DELETE FROM channel_users_by_user_id WHERE userid = ? AND channelid = ?");

        _selectByChannelId = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channelid = ?");

        _selectByChannelIds = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channelid IN ?");

        _selectByUserId = session.Prepare("SELECT * FROM channel_users_by_user_id WHERE userid = ?");

        _selectByChannelIdAndUserIds = session.Prepare("SELECT * FROM channel_users_by_channel_id WHERE channelid = ? AND userid IN ?");

        _updateReadAt = session.Prepare("UPDATE channel_users_by_user_id SET readat = ? WHERE userid = ? AND channelid = ?");
    }

    public BoundStatement Insert(long channelId, ChannelMemberInfo member)
    {
        return _insert.Bind(
            member.UserId,
            channelId,
            member.ReadAt,
            member.Username,
            member.GlobalName,
            member.Image,
            (long)member.Permissions.ToValue());
    }

    public BoundStatement Delete(long userId, long channelId)
    {
        return _delete.Bind(userId, channelId);
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

    public BoundStatement UpdateReadAt(long userId, long channelId, long readAt)
    {
        return _updateReadAt.Bind(readAt, userId, channelId);
    }
}
