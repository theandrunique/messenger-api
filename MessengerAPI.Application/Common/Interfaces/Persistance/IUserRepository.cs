using MessengerAPI.Domain.User;
using MessengerAPI.Domain.User.Entities;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task Commit();
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<(Session, User)?> GetSessionWithUserByTokenId(Guid tokenId);
}
