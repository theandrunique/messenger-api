using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Users;

internal class UserRepository : IUserRepository
{
    private readonly ISession _session;
    private readonly UserQueries _users;

    public UserRepository(ISession session, UserQueries users)
    {
        _session = session;
        _users = users;
    }

    public Task AddAsync(User user)
    {
        return _session.ExecuteAsync(_users.Insert(user));
    }

    public async Task<User?> GetByEmailOrNullAsync(string email)
    {
        var result = (await _session.ExecuteAsync(_users.SelectByEmail(email)))
            .FirstOrDefault();
        return MapOrDefault(result);
    }

    public async Task<User?> GetByIdOrNullAsync(long id)
    {
        var result = (await _session.ExecuteAsync(_users.SelectById(id)))
            .FirstOrDefault();
        return MapOrDefault(result);
    }

    public async Task<User?> GetByUsernameOrNullAsync(string username)
    {
        var result = (await _session.ExecuteAsync(_users.SelectByUsername(username)))
            .FirstOrDefault();
        return MapOrDefault(result);
    }

    public async Task<IEnumerable<User>> GetByIdsAsync(List<long> userIds)
    {
        var result = await _session.ExecuteAsync(_users.SelectByIds(userIds));
        return result.Select(UserMapper.Map);
    }

    public Task UpdateEmailAsync(
        long userId,
        string email,
        DateTimeOffset emailUpdatedTimestamp,
        bool isEmailVerified)
    {
        return _session.ExecuteAsync(_users.UpdateEmail(userId, email, emailUpdatedTimestamp, isEmailVerified));
    }

    public Task SetEmailVerifiedAsync(long userId)
    {
        return _session.ExecuteAsync(_users.UpdateEmailVerified(userId, true));
    }

    public Task UpdateMfaStatusAsync(User user)
    {
        return _session.ExecuteAsync(_users.UpdateMfaStatus(user));
    }

    private User? MapOrDefault(Row? row)
    {
        return row is null ? null : UserMapper.Map(row);
    }
}
