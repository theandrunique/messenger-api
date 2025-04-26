using Cassandra;

namespace Messenger.Data.Implementations.Users.Queries;

public class UsersByUsernameQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _select;
    private readonly PreparedStatement _delete;

    public UsersByUsernameQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO auth.users_by_username (username, userid)
            VALUES (?, ?)
            IF NOT EXISTS
        """);

        _select = session.Prepare("SELECT userid FROM auth.users_by_username WHERE username = ?");

        _delete = session.Prepare("DELETE FROM auth.users_by_username WHERE username = ?");
    }

    public BoundStatement InsertIfNotExists(string username, long userId)
    {
        return _insert.Bind(username, userId);
    }

    public BoundStatement Select(string username)
    {
        return _select.Bind(username);
    }

    public BoundStatement Delete(string username)
    {
        return _delete.Bind(username);
    }
}
