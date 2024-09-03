using MessengerAPI.Domain.User;

namespace MessengerAPI.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task<User?> AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
}
