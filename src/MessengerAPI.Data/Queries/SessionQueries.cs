using Cassandra;
using Session = MessengerAPI.Domain.Entities.Session;

namespace MessengerAPI.Data.Queries;

internal class SessionQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByUserIdAndId;
    private readonly PreparedStatement _selectByTokenId;
    private readonly PreparedStatement _updateTokenId;

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
        _updateTokenId = session.Prepare("UPDATE sessions SET lastusedtimestamp = ?, tokenid = ? WHERE userid = ? AND id = ?");
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

    public BoundStatement UpdateTokenId(Session session)
    {
        return _updateTokenId.Bind(session.LastUsedTimestamp, session.TokenId, session.UserId, session.Id);
    }
}
