using Messenger.Domain.Entities;

namespace Messenger.Data.Interfaces.Users;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session?> GetByIdOrNullAsync(long userId, long sessionId);
    Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId);
    Task<List<Session>> GetSessionsByUserId(long userId);
    Task UpdateTokenIdAsync(Session session);
    Task RemoveByIdAsync(long userId, long sessionId);
}
