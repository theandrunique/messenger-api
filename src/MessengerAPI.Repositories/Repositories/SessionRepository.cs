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
        _table.CreateIfNotExists();
    }

    public Task AddAsync(Session session)
    {
        return _table.Insert(session).ExecuteAsync();
    }

    public Task<Session> GetByIdOrDefaultAsync(Guid userId, Guid sessionId)
    {
        return _table
            .Where(s => s.UserId == userId && s.Id == sessionId)
            .FirstOrDefault()
            .ExecuteAsync();
    }

    public Task<Session> GetByTokenIdOrDefaultAsync(Guid tokenId)
    {
        return _table
            .Where(s => s.TokenId == tokenId)
            .FirstOrDefault()
            .ExecuteAsync();
    }

    public Task RemoveByIdAsync(Guid userId, Guid sessionId)
    {
        return _table
            .Where(s => s.UserId == userId && s.Id == sessionId)
            .Delete()
            .ExecuteAsync();
    }

    public Task UpdateTokenIdAsync(Guid userId, Guid sessionId, Guid tokenId)
    {
        var query = $"UPDATE sessions SET {nameof(Session.TokenId)} = ? WHERE {nameof(Session.UserId)} = ? AND {nameof(Session.Id)} = ?";

        return _session.ExecuteAsync(new SimpleStatement(query, tokenId, userId, sessionId));
    }
}
