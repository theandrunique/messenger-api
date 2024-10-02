using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    /// <summary>
    /// Save changes to database
    /// </summary>
    /// <param name="token"><see cref="CancellationToken"/></param>
    Task CommitAsync(CancellationToken token);
    /// <summary>
    /// Add new user
    /// </summary>
    /// <param name="user"><see cref="User"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    Task AddAsync(User user, CancellationToken token);
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"><see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns><see cref="User?"/></returns>
    Task<User?> GetByIdOrNullAsync(UserId id, CancellationToken token);
    /// <summary>
    /// Get user by ids
    /// </summary>
    /// <param name="ids">list of <see cref="UserId"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>list of <see cref="User"/></returns>
    Task<List<User>> GetByIdsAsync(List<UserId> ids, CancellationToken token);
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
