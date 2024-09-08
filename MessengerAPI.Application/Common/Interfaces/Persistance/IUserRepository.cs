using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task Commit();
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(UserId id);
    Task<List<User>> GetByIdsAsync(List<UserId> id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<(Session, User)?> GetSessionWithUserByTokenId(Guid tokenId);
}
