using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Users;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session> GetByIdOrDefaultAsync(long userId, long sessionId);
    Task<Session> GetByTokenIdOrDefaultAsync(Guid tokenId);
    Task UpdateTokenIdAsync(long userId, long sessionId, Guid tokenId);
    Task RemoveByIdAsync(long userId, long sessionId);
}