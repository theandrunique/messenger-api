using Cassandra;
using Messenger.Domain.Entities;

namespace Messenger.Data.Implementations.Channels.Queries;

public class PrivateChannelQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByUserIds;

    public PrivateChannelQueries(ISession session)
    {
        _insert = session.Prepare("INSERT INTO private_channels (userid1, userid2, channelid) VALUES (?, ?, ?)");

        _selectByUserIds = session.Prepare("SELECT * FROM private_channels WHERE userid1 = ? AND userid2 = ?");
    }

    public BoundStatement Insert(Channel channel)
    {
        var sortedMembers = channel.AllMembers.OrderBy(member => member.UserId).ToList();

        if (sortedMembers.Count == 2)
        {
            return _insert.Bind(sortedMembers[0].UserId, sortedMembers[1].UserId, channel.Id);
        }
        else if (sortedMembers.Count == 1)
        {
            return _insert.Bind(sortedMembers[0].UserId, sortedMembers[0].UserId, channel.Id);
        }
        else
        {
            throw new ArgumentException("Private channel must have exactly 1 or 2 members.");
        }
    }

    public BoundStatement SelectByUserIds(long userId1, long userId2)
    {
        if (userId1.CompareTo(userId2) > 0)
        {
            (userId1, userId2) = (userId2, userId1);
        }
        return _selectByUserIds.Bind(userId1, userId2);
    }
}
