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

    public Task AddAsync(Session session, CancellationToken token)
    {
        var statement = _table.Insert(session);
        return statement.ExecuteAsync();
    }

    public Task RemoveAsync(Guid sessionId, CancellationToken token)
    {
        return _table.Where(s => s.Id == sessionId).Delete().ExecuteAsync();
    }

    public Task UpdateAsync(Session session, CancellationToken token)
    {
        return _table.Where(s => s.Id == session.Id).Update().ExecuteAsync();
    }
}
