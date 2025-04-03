using Cassandra;
using Session = MessengerAPI.Domain.Entities.Session;

namespace MessengerAPI.Data.Implementations.Queries;

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
            INSERT INTO sessions (
                userid,
                id,
                clientname,
                timestamp,
                devicename,
                lastusedtimestamp,
                location,
                tokenid) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        """);

        _selectByUserIdAndId = session.Prepare("SELECT * FROM sessions WHERE userid = ? AND id = ?");
        _selectByTokenId = session.Prepare("SELECT * FROM sessions WHERE tokenid = ?");
        _selectByUserId = session.Prepare("SELECT * FROM sessions WHERE userid = ?");
        _updateTokenId = session.Prepare("UPDATE sessions SET lastusedtimestamp = ?, tokenid = ? WHERE userid = ? AND id = ?");
        _deleteByTokenId = session.Prepare("DELETE FROM sessions WHERE userid = ? AND id = ?");
    }

    public BoundStatement Insert(Session session)
    {
        return _insert.Bind(
            session.UserId,
            session.Id,
            session.ClientName,
            session.Timestamp,
            session.DeviceName,
            session.LastUsedTimestamp,
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
        return _updateTokenId.Bind(session.LastUsedTimestamp, session.TokenId, session.UserId, session.Id);
    }

    public BoundStatement DeleteByTokenId(long userId, long sessionId)
    {
        return _deleteByTokenId.Bind(userId, sessionId);
    }
}
