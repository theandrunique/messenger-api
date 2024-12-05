using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Repositories.Interfaces;
using Session = MessengerAPI.Domain.Models.Entities.Session;

namespace MessengerAPI.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ISession _session;
    private readonly Table<Session> _table;

    public SessionRepository(ISession session)
    {
        _session = session;
        _table = new Table<Session>(_session);
    }

    public Task AddAsync(Session session)
    {
        var statement = _table.Insert(session);
        return statement.ExecuteAsync();
    }

    public Task<Session?> GetByIdOrNullAsync(Guid sessionId)
    {
        return _table.Where(s => s.Id == sessionId).FirstOrDefault().ExecuteAsync();
    }

    public Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId)
    {
        return _table.Where(s => s.TokenId == tokenId).FirstOrDefault().ExecuteAsync();
    }

    public Task RemoveByIdAsync(Guid sessionId)
    {
        return _table.Where(s => s.Id == sessionId).Delete().ExecuteAsync();
    }

    public Task UpdateTokenIdAsync(Guid sessionId, Guid tokenId)
    {
        var query = $"UPDATE sessions SET {nameof(Session.TokenId)} = ? WHERE {nameof(Session.Id)} = ?";

        return _session.ExecuteAsync(new SimpleStatement(query, tokenId, sessionId));
    }
}
