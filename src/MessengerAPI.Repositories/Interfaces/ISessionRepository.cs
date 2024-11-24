using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface ISessionRepository
{
    Task AddAsync(Session session);
    Task<Session?> GetByIdOrNullAsync(Guid sessionId);
    Task<Session?> GetByTokenIdOrNullAsync(Guid tokenId);
    Task UpdateTokenIdAsync(Guid sessionId, Guid tokenId);
    Task RemoveByIdAsync(Guid sessionId);
}