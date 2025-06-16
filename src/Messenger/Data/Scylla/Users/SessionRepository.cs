using Cassandra;
using Cassandra.Data.Linq;
using Messenger.Data.Scylla.Users.Mappers;
using Messenger.Data.Scylla.Users.Queries;
using Messenger.Data.Interfaces.Users;
using Session = Messenger.Domain.Auth.Session;

namespace Messenger.Data.Scylla.Users;

public class SessionRepository : ISessionRepository
{
    private readonly ISession _session;
    private readonly SessionQueries _sessions;

    public SessionRepository(ISession session, SessionQueries sessions)
    {
        _session = session;
        _sessions = sessions;
    }

    public Task AddAsync(Session session)
    {
        return _session.ExecuteAsync(_sessions.Insert(session));
    }

    public async Task<Session?> GetByIdOrNullAsync(long userId, long sessionId)
    {
        var result = (await _session.ExecuteAsync(_sessions.SelectByUserIdAndId(userId, sessionId))).FirstOrDefault();
        return MapOrDefault(result);
    }

    public async Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId)
    {
        var result = (await _session.ExecuteAsync(_sessions.SelectByTokenId(tokenId))).FirstOrDefault();
        return MapOrDefault(result);
    }

    public async Task<List<Session>> GetSessionsByUserId(long userId)
    {
        var result = await _session.ExecuteAsync(_sessions.SelectByUserId(userId));
        return result.Select(SessionMapper.Map).ToList();
    }

    private Session? MapOrDefault(Row? row)
    {
        return row is null ? null : SessionMapper.Map(row);
    }

    public Task UpdateTokenIdAsync(Session session)
    {
        return _session.ExecuteAsync(_sessions.UpdateTokenId(session));
    }

    public Task RemoveByIdAsync(long userId, long sessionId)
    {
        return _session.ExecuteAsync(_sessions.DeleteByTokenId(userId, sessionId));
    }
}
