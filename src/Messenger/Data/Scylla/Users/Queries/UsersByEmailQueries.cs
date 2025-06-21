using Cassandra;

namespace Messenger.Data.Scylla.Users.Queries;

public class UsersByEmailQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _select;
    private readonly PreparedStatement _delete;

    public UsersByEmailQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO auth.users_by_email (email, user_id) VALUES (?, ?)
            IF NOT EXISTS
        """);

        _select = session.Prepare("SELECT user_id FROM auth.users_by_email WHERE email = ?");

        _delete = session.Prepare("DELETE FROM auth.users_by_email WHERE email = ?");
    }

    public BoundStatement InsertIfNotExists(string email, long userId)
    {
        return _insert.Bind(email, userId);
    }

    public BoundStatement Select(string email)
    {
        return _select.Bind(email);
    }

    public BoundStatement Delete(string email)
    {
        return _delete.Bind(email);
    }
}
