using Cassandra;
using Cassandra.Data.Linq;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Data.Scylla.Users.Mappers;
using Messenger.Data.Scylla.Users.Queries;
using Messenger.Data.Interfaces.Users;
using Microsoft.Extensions.Logging;
using Messenger.Domain.Auth;

namespace Messenger.Data.Scylla.Users;

public class UserRepository : IUserRepository
{
    private readonly ISession _session;
    private readonly UserQueries _users;
    private readonly ChannelUserQueries _channelUsers;
    private readonly UsersByEmailQueries _usersByEmail;
    private readonly UsersByUsernameQueries _usersByUsername;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        ISession session,
        UserQueries users,
        ChannelUserQueries channelUsers,
        UsersByEmailQueries usersByEmail,
        UsersByUsernameQueries usersByUsername,
        ILogger<UserRepository> logger)
    {
        _session = session;
        _users = users;
        _channelUsers = channelUsers;
        _usersByEmail = usersByEmail;
        _usersByUsername = usersByUsername;
        _logger = logger;
    }

    public async Task<bool> AddAsync(User user)
    {
        var result1 = (await _session.ExecuteAsync(_usersByUsername.InsertIfNotExists(user.Username, user.Id)))
            .First()
            .GetValue<bool>("[applied]");

        if (!result1) return false;

        var result2 = (await _session.ExecuteAsync(_usersByEmail.InsertIfNotExists(user.Email, user.Id)))
            .First()
            .GetValue<bool>("[applied]");

        if (!result2)
        {
            await _session.ExecuteAsync(_usersByEmail.Delete(user.Email));
            return false;
        }

        try
        {
            await _session.ExecuteAsync(_users.Insert(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting user {UserId}", user.Id);

            await _session.ExecuteAsync(_usersByUsername.Delete(user.Username));
            await _session.ExecuteAsync(_usersByEmail.Delete(user.Email));
            return false;
        }

        return true;
    }

    public async Task<User?> GetByIdOrNullAsync(long userId)
    {
        var result = (await _session.ExecuteAsync(_users.SelectById(userId)))
            .FirstOrDefault();

        return MapOrDefault(result);
    }

    public async Task<IEnumerable<User>> GetByIdsAsync(List<long> userIds)
    {
        return (await _session.ExecuteAsync(_users.SelectByIds(userIds)))
            .Select(UserMapper.Map);
    }

    public async Task<User?> GetByEmailOrNullAsync(string email)
    {
        var userId = (await _session.ExecuteAsync(_usersByEmail.Select(email)))
            .FirstOrDefault()
            ?.GetValue<long>("user_id");

        if (userId == null) return null;

        var result = (await _session.ExecuteAsync(_users.SelectById(userId.Value)))
            .First();

        return UserMapper.Map(result);
    }

    public async Task<User?> GetByUsernameOrNullAsync(string username)
    {
        var userId = (await _session.ExecuteAsync(_usersByUsername.Select(username)))
            .FirstOrDefault()
            ?.GetValue<long>("user_id");

        if (userId == null) return null;

        var result = (await _session.ExecuteAsync(_users.SelectById(userId.Value)))
            .First();

        return UserMapper.Map(result);
    }

    public async Task<bool> IsExistsByEmailAsync(string email)
    {
        var userId = (await _session.ExecuteAsync(_usersByEmail.Select(email)))
            .FirstOrDefault()
            ?.GetValue<long>("user_id");

        return userId != null;
    }

    public async Task<bool> IsExistsByUsernameAsync(string username)
    {
        var userId = (await _session.ExecuteAsync(_usersByUsername.Select(username)))
            .FirstOrDefault()
            ?.GetValue<long>("user_id");

        return userId != null;
    }

    public async Task UpdateAvatarAsync(User user)
    {
        var channelIds = (await _session.ExecuteAsync(_channelUsers.SelectChannelIdsByUserId(user.Id)))
            .Select(row => row.GetValue<long>("channel_id"));

        var batch = new BatchStatement()
            .Add(_users.UpdateAvatar(user.Id, user.Image))
            .Add(_channelUsers.UpdateUserInfo(user, channelIds));

        await _session.ExecuteAsync(batch);
    }

    public async Task<bool> UpdateEmailAsync(
        long userId,
        string email,
        string oldEmail,
        DateTimeOffset emailUpdatedTimestamp,
        bool isEmailVerified)
    {
        var isSuccess = (await _session.ExecuteAsync(_usersByEmail.InsertIfNotExists(email, userId)))
            .First()
            .GetValue<bool>("[applied]");

        if (!isSuccess) return false;

        var batch = new BatchStatement()
            .Add(_usersByEmail.Delete(oldEmail))
            .Add(_users.UpdateEmail(userId, email, emailUpdatedTimestamp, isEmailVerified));

        await _session.ExecuteAsync(batch);
        return true;
    }

    public Task UpdateIsEmailVerifiedAsync(long userId, bool isEmailVerified)
    {
        return _session.ExecuteAsync(_users.UpdateIsEmailVerified(userId, isEmailVerified));
    }

    public Task UpdateTotpMfaInfoAsync(User user)
    {
        return _session.ExecuteAsync(_users.UpdateTotpMfaStatus(user));
    }

    public async Task<bool> UpdateUsernameAsync(
        long userId,
        string username,
        string oldUsername,
        DateTimeOffset usernameUpdatedTimestamp)
    {
        var isSuccess = (await _session.ExecuteAsync(_usersByUsername.InsertIfNotExists(username, userId)))
            .First()
            .GetValue<bool>("[applied]");

        if (!isSuccess) return false;

        var channelIds = (await _session.ExecuteAsync(_channelUsers.SelectChannelIdsByUserId(userId)))
            .Select(row => row.GetValue<long>("channel_id"));

        var batch = new BatchStatement()
            .Add(_usersByUsername.Delete(oldUsername))
            .Add(_users.UpdateUsernameInfo(username, usernameUpdatedTimestamp, userId))
            .Add(_channelUsers.UpdateUsername(username, channelIds));

        await _session.ExecuteAsync(batch);
        return true;
    }

    private User? MapOrDefault(Row? row)
    {
        return row is null ? null : UserMapper.Map(row);
    }
}
