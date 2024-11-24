using System.Reflection.Metadata;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdOrNullAsync(Guid userId);
    Task<User?> GetByEmailOrNullAsync(string email);
    Task<User?> GetByUsernameOrNullAsync(string username);
    Task<IEnumerable<User>> GetByIdsAsync(List<Guid> members);
}