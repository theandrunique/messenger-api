using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session?> GetByIdOrNullAsync(Guid userId, Guid sessionId);
    Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId);
    Task UpdateTokenIdAsync(Guid userId, Guid sessionId, Guid tokenId);
    Task RemoveByIdAsync(Guid userId, Guid sessionId);
}