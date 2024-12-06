using System.Reflection.Metadata;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User> GetByIdOrDefaultAsync(Guid userId);
    Task<User> GetByEmailOrDefaultAsync(string email);
    Task<User> GetByUsernameOrDefaultAsync(string username);
    Task<IEnumerable<User>> GetByIdsAsync(List<Guid> members);
    Task SetEmailVerifiedAsync(Guid userId);
}