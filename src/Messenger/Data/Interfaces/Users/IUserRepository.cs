using Messenger.Domain.Entities;

namespace Messenger.Data.Interfaces.Users;

public interface IUserRepository
{
    Task<bool> AddAsync(User user);

    Task<User?> GetByIdOrNullAsync(long userId);
    Task<User?> GetByEmailOrNullAsync(string email);
    Task<User?> GetByUsernameOrNullAsync(string username);
    Task<IEnumerable<User>> GetByIdsAsync(List<long> userIds);

    Task<bool> IsExistsByEmailAsync(string email);
    Task<bool> IsExistsByUsernameAsync(string email);

    Task<bool> UpdateEmailAsync(
        long userId,
        string email,
        string oldEmail,
        DateTimeOffset emailUpdatedTimestamp,
        bool isEmailVerified);
    Task<bool> UpdateUsernameAsync(
        long userId,
        string username,
        string oldUsername,
        DateTimeOffset usernameUpdatedTimestamp);

    Task UpdateIsEmailVerifiedAsync(long userId, bool isEmailVerified);
    Task UpdateTotpMfaInfoAsync(User user);
    Task UpdateAvatarAsync(User user);
}
