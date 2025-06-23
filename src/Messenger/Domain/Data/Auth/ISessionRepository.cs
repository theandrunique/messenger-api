using Messenger.Domain.Auth;

namespace Messenger.Domain.Data.Auth;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session?> GetByIdOrNullAsync(long userId, long sessionId);
    Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId);
    Task<List<Session>> GetSessionsByUserId(long userId);
    Task UpdateTokenIdAsync(Session session);
    Task RemoveByIdAsync(long userId, long sessionId);
}
