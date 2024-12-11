using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Users;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User> GetByIdOrDefaultAsync(Guid userId);
    Task<User> GetByEmailOrDefaultAsync(string email);
    Task<User> GetByUsernameOrDefaultAsync(string username);
    Task<IEnumerable<User>> GetByIdsAsync(List<Guid> members);
    Task SetEmailVerifiedAsync(Guid userId);
    Task UpdateKeyAsync(User user);
    Task UpdatePasswordAsync(User user);
}