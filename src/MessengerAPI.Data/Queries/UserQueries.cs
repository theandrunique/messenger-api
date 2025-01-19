using Cassandra;

namespace MessengerAPI.Data.Queries;

internal class UserQueries
{
    private readonly PreparedStatement _selectUserInfoForMessage;
    private readonly PreparedStatement _selectUsersInfoForMessages;

    public UserQueries(ISession session)
    {
        _selectUserInfoForMessage = session.Prepare("SELECT id, username, globalname, image FROM users WHERE id = ?");
        _selectUsersInfoForMessages = session.Prepare("SELECT id, username, globalname, image FROM users WHERE id IN ?");
    }

    public BoundStatement SelectUserInfoForMessage(long userId)
    {
        return _selectUserInfoForMessage.Bind(userId);
    }

    public BoundStatement SelectUsersInfoForMessages(IEnumerable<long> userIds)
    {
        return _selectUsersInfoForMessages.Bind(userIds);
    }
}