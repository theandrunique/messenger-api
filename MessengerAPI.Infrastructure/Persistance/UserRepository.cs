using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.User;

namespace MessengerAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    public Task<User?> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUsernameAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}
