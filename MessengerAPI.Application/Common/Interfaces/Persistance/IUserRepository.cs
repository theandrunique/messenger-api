using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task Commit(CancellationToken token);
    Task AddAsync(User user, CancellationToken token);
    Task<User?> GetByIdAsync(UserId id, CancellationToken token);
    Task<List<User>> GetByIdsAsync(List<UserId> id, CancellationToken token);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
    Task<User?> GetByUsernameAsync(string username, CancellationToken token);
    Task<(Session, User)?> GetSessionWithUserByTokenId(Guid tokenId, CancellationToken token);
}
