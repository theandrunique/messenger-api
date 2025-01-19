using Cassandra;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Queries;

internal class PrivateChannelQueries
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
        if (channel.Members.Count != 2)
        {
            throw new ArgumentException("Private channel must have exactly two members.");
        }

        var sortedMembers = channel.Members.OrderBy(member => member.UserId).ToList();

        return _insert.Bind(sortedMembers[0].UserId, sortedMembers[1].UserId, channel.Id);
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
