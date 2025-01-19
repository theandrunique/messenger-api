using Cassandra;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Queries;

internal class SavedMessagesChannelQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByUserId;

    public SavedMessagesChannelQueries(ISession session)
    {
        _insert = session.Prepare("INSERT INTO saved_messages_channel (userid, channelid) VALUES (?, ?)");
        _selectByUserId = session.Prepare("SELECT * FROM saved_messages_channel WHERE userid = ?");
    }

    public BoundStatement Insert(Channel channel)
    {
        if (channel.Members.Count != 1)
        {
            throw new ArgumentException("SavedMessages channel must have exactly one member.");
        }

        return _insert.Bind(channel.Members[0].UserId, channel.Id);
    }

    public BoundStatement SelectByUserId(long userId)
    {
        return _selectByUserId.Bind(userId);
    }
}
