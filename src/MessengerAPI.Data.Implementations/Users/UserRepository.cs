using Cassandra;
using Cassandra.Data.Linq;
using MessengerAPI.Data.Implementations.Mappers;
using MessengerAPI.Data.Implementations.Queries;
using MessengerAPI.Data.Interfaces.Users;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Users;

public class UserRepository : IUserRepository
{
    private readonly ISession _session;
    private readonly UserQueries _users;
    private readonly ChannelUserQueries _channelUsers;

    public UserRepository(ISession session, UserQueries users, ChannelUserQueries channelUsers)
    {
        _session = session;
        _users = users;
        _channelUsers = channelUsers;
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

    public async Task UpdateAvatarAsync(User user)
    {
        await _session.ExecuteAsync(_users.UpdateAvatar(user.Id, user.Image));

        var channelIds = (await _session.ExecuteAsync(_channelUsers.SelectChannelIdsByUserId(user.Id)))
            .Select(row => row.GetValue<long>("channelid"));
        
        await _session.ExecuteAsync(_channelUsers.UpdateUserInfo(user, channelIds));
    }

    private User? MapOrDefault(Row? row)
    {
        return row is null ? null : UserMapper.Map(row);
    }
}
