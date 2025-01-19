using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Users;

internal class UserRepository : IUserRepository
{
    private readonly ISession _session;
    private readonly Table<User> _table;

    public UserRepository(ISession session)
    {
        _session = session;
        _table = new Table<User>(_session);
    }

    public Task AddAsync(User user)
    {
        return _table.Insert(user).ExecuteAsync();
    }

    public Task<User> GetByEmailOrDefaultAsync(string email)
    {
        return _table
            .Where(u => u.Email == email)
            .FirstOrDefault()
            .ExecuteAsync();
    }

    public Task<User> GetByIdOrDefaultAsync(long id)
    {
        return _table
            .Where(u => u.Id == id)
            .FirstOrDefault()
            .ExecuteAsync();
    }

    public Task<IEnumerable<User>> GetByIdsAsync(List<long> members)
    {
        return _table
            .Where(u => members.Contains(u.Id))
            .ExecuteAsync();
    }

    public Task<User> GetByUsernameOrDefaultAsync(string username)
    {
        return _table
            .Where(u => u.Username == username)
            .FirstOrDefault()
            .ExecuteAsync();
    }

    public Task SetEmailVerifiedAsync(long userId)
    {
        var statement = $"UPDATE users SET {nameof(User.IsEmailVerified)} = true WHERE {nameof(User.Id)} = ?";

        return _session.ExecuteAsync(new SimpleStatement(statement, userId));
    }

    public Task UpdateKeyAsync(User user)
    {
        var statement = $"UPDATE users SET {nameof(User.TOTPKey)} = ? WHERE {nameof(User.Id)} = ?";

        return _session.ExecuteAsync(new SimpleStatement(statement, user.TOTPKey, user.Id));
    }

    public Task UpdatePasswordAsync(User user)
    {
        var statement = $"""
            UPDATE
                users
            SET
                {nameof(User.PasswordHash)} = ?,
                {nameof(User.PasswordUpdatedTimestamp)} = ?
            WHERE
                {nameof(User.Id)} = ?
        """;

        return _session.ExecuteAsync(new SimpleStatement(statement, user.PasswordHash, user.PasswordUpdatedTimestamp, user.Id));
    }
}

