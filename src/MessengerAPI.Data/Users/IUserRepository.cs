using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Users;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdOrNullAsync(long userId);
    Task<User?> GetByEmailOrNullAsync(string email);
    Task<User?> GetByUsernameOrNullAsync(string username);
    Task<IEnumerable<User>> GetByIdsAsync(List<long> userIds);
    Task UpdateEmailAsync(
        long userId,
        string email,
        DateTimeOffset emailUpdatedTimestamp,
        bool isEmailVerified);
    Task SetEmailVerifiedAsync(long userId);
    Task UpdateTOTPKeyAsync(User user);
}
