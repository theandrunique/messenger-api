using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    /// <summary>
    /// Add new user
    /// </summary>
    /// <param name="user"><see cref="User"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    Task AddAsync(User user, CancellationToken token);
    /// <summary>
    /// Add new session
    /// </summary>
    /// <param name="session"><see cref="Session"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    Task AddSessionAsync(Session session, CancellationToken token);
    /// <summary>
    /// Update session
    /// </summary>
    /// <param name="session"><see cref="Session"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task UpdateSessionAsync(Session session, CancellationToken token);
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"><see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="User?"/></returns>
    Task<User?> GetByIdOrNullAsync(Guid id, CancellationToken token);
    /// <summary>
    /// Get user by ids
    /// </summary>
    /// <param name="ids">list of <see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>list of <see cref="User"/></returns>
    Task<List<User>> GetByIdsAsync(List<Guid> ids, CancellationToken token);
    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="User?"/></returns>
    Task<User?> GetByEmailOrNullAsync(string email, CancellationToken token);
    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="User?"/></returns>
    Task<User?> GetByUsernameOrNullAsync(string username, CancellationToken token);
    /// <summary>
    /// Get session with user by token id
    /// </summary>
    /// <param name="tokenId">Token id</param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Session?"/> and <see cref="User?"/></returns>
    Task<(Session?, User?)> GetSessionWithUserByTokenIdOrNullAsync(Guid tokenId, CancellationToken token);
}
