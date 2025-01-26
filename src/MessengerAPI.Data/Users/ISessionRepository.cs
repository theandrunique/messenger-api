using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Users;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session?> GetByIdOrNullAsync(long userId, long sessionId);
    Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId);
    Task UpdateTokenIdAsync(Session session);
}
