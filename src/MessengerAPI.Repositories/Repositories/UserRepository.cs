using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Repositories.Interfaces;

namespace MessengerAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISession _session;
    private readonly Table<User> _table;

    public UserRepository(ISession session)
    {
        _session = session;
        _table = new Table<User>(_session);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        var statement = _table.Insert(user);
        return statement.ExecuteAsync();
    }

    public Task<User?> GetByEmailOrNullAsync(string email, CancellationToken cancellationToken)
    {
        var statement = _table.Where(u => u.Email == email);
        return statement.FirstOrDefault().ExecuteAsync();
    }

    public Task<User?> GetByIdOrNullAsync(Guid id, CancellationToken cancellationToken)
    {
        var statement = _table.Where(u => u.Id == id);
        return statement.FirstOrDefault().ExecuteAsync();
    }

    public Task<User?> GetByUsernameOrNullAsync(string username, CancellationToken cancellationToken)
    {
        var statement = _table.Where(u => u.Username == username);
        return statement.FirstOrDefault().ExecuteAsync();
    }
}

