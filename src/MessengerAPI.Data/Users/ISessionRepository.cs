using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Users;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session> GetByIdOrDefaultAsync(Guid userId, Guid sessionId);
    Task<Session> GetByTokenIdOrDefaultAsync(Guid tokenId);
    Task UpdateTokenIdAsync(Guid userId, Guid sessionId, Guid tokenId);
    Task RemoveByIdAsync(Guid userId, Guid sessionId);
}