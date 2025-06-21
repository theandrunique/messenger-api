using Cassandra;
using Session = Messenger.Domain.Auth.Session;

namespace Messenger.Data.Scylla.Users.Queries;

public class SessionQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByUserIdAndId;
    private readonly PreparedStatement _selectByTokenId;
    private readonly PreparedStatement _selectByUserId;
    private readonly PreparedStatement _updateTokenId;
    private readonly PreparedStatement _deleteByTokenId;

    public SessionQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO auth.sessions (
                user_id,
                session_id,
                client_name,
                timestamp,
                device_name,
                last_used_timestamp,
                location,
                token_id
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        """);

        _selectByUserIdAndId = session.Prepare("SELECT * FROM auth.sessions WHERE user_id = ? AND session_id = ?");
        _selectByTokenId = session.Prepare("SELECT * FROM auth.sessions WHERE token_id = ?");
        _selectByUserId = session.Prepare("SELECT * FROM auth.sessions WHERE user_id = ?");
        _updateTokenId = session.Prepare("UPDATE auth.sessions SET last_used_timestamp = ?, token_id = ? WHERE user_id = ? AND session_id = ?");
        _deleteByTokenId = session.Prepare("DELETE FROM auth.sessions WHERE user_id = ? AND session_id = ?");
    }

    public BoundStatement Insert(Session session)
    {
        return _insert.Bind(
            session.UserId,
            session.Id,
            session.ClientName,
            session.Timestamp,
            session.DeviceName,
            session.LastRefreshTimestamp,
            session.Location,
            session.TokenId);
    }

    public BoundStatement SelectByUserIdAndId(long userId, long sessionId)
    {
        return _selectByUserIdAndId.Bind(userId, sessionId);
    }

    public BoundStatement SelectByTokenId(Guid tokenId)
    {
        return _selectByTokenId.Bind(tokenId);
    }

    public BoundStatement SelectByUserId(long userId)
    {
        return _selectByUserId.Bind(userId);
    }

    public BoundStatement UpdateTokenId(Session session)
    {
        return _updateTokenId.Bind(session.LastRefreshTimestamp, session.TokenId, session.UserId, session.Id);
    }

    public BoundStatement DeleteByTokenId(long userId, long sessionId)
    {
        return _deleteByTokenId.Bind(userId, sessionId);
    }
}
