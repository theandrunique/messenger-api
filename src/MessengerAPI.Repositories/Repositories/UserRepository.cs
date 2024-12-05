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
        _table.CreateIfNotExists();
    }

    public Task AddAsync(User user)
    {
        var statement = _table.Insert(user);
        return statement.ExecuteAsync();
    }

    public Task<User?> GetByEmailOrNullAsync(string email)
    {
        var statement = _table.Where(u => u.Email == email);
        return statement.FirstOrDefault().ExecuteAsync();
    }

    public Task<User?> GetByIdOrNullAsync(Guid id)
    {
        var statement = _table.Where(u => u.Id == id);
        return statement.FirstOrDefault().ExecuteAsync();
    }

    public Task<IEnumerable<User>> GetByIdsAsync(List<Guid> members)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUsernameOrNullAsync(string username)
    {
        var statement = _table.Where(u => u.Username == username);
        return statement.FirstOrDefault().ExecuteAsync();
    }

    public Task SetEmailVerifiedAsync(Guid userId)
    {
        var statement = $"UPDATE users SET {nameof(User.IsEmailVerified)} = true WHERE {nameof(User.Id)} = ?";

        return _session.ExecuteAsync(new SimpleStatement(statement, userId));
    }
}

