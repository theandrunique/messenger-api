using Cassandra;
using Messenger.Domain.Auth;

namespace Messenger.Data.Scylla.Users.Queries;

public class UserQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByIds;
    private readonly PreparedStatement _updateEmailInfo;
    private readonly PreparedStatement _updateUsernameInfo;
    private readonly PreparedStatement _updateMfaStatusKey;
    private readonly PreparedStatement _updateEmailVerified;
    private readonly PreparedStatement _updateAvatar;

    public UserQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO auth.users (
                user_id,
                bio,
                timestamp,
                sessions_lifetime,
                email,
                email_updated_timestamp,
                global_name,
                is_active,
                is_email_verified,
                totp_key,
                password_hash,
                password_updated_timestamp,
                two_factor_authentication,
                username,
                username_updated_timestamp,
                image
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """);

        _selectById = session.Prepare("SELECT * FROM auth.users WHERE user_id = ?");

        _selectByIds = session.Prepare("SELECT * FROM auth.users WHERE user_id IN ?");

        _updateEmailInfo = session.Prepare("UPDATE auth.users SET email = ?, email_updated_timestamp = ?, is_email_verified = ? WHERE user_id = ?");

        _updateUsernameInfo = session.Prepare("UPDATE auth.users SET username = ?, username_updated_timestamp = ? WHERE user_id = ?");

        _updateMfaStatusKey = session.Prepare("UPDATE auth.users SET totp_key = ?, two_factor_authentication = ? WHERE user_id = ?");

        _updateEmailVerified = session.Prepare("UPDATE auth.users SET is_email_verified = ? WHERE user_id = ?");

        _updateAvatar = session.Prepare("UPDATE auth.users SET image = ? WHERE user_id = ?");
    }

    public BoundStatement Insert(User user)
    {
        return _insert.Bind(
            user.Id,
            user.Bio,
            user.Timestamp,
            (int)user.SessionsLifetime,
            user.Email,
            user.EmailUpdatedTimestamp,
            user.GlobalName,
            user.IsActive,
            user.IsEmailVerified,
            user.TOTPKey,
            user.PasswordHash,
            user.PasswordUpdatedTimestamp,
            user.TwoFactorAuthentication,
            user.Username,
            user.UsernameUpdatedTimestamp,
            user.Image);
    }

    public BoundStatement SelectById(long id)
    {
        return _selectById.Bind(id);
    }

    public BoundStatement SelectByIds(IEnumerable<long> ids)
    {
        return _selectByIds.Bind(ids);
    }

    public BoundStatement UpdateEmail(
        long userId,
        string email,
        DateTimeOffset emailUpdatedTimestamp,
        bool isEmailVerified)
    {
        return _updateEmailInfo.Bind(email, emailUpdatedTimestamp, isEmailVerified, userId);
    }

    public BoundStatement UpdateUsernameInfo(string username, DateTimeOffset usernameUpdatedAt, long userId)
    {
        return _updateUsernameInfo.Bind(username, usernameUpdatedAt, userId);
    }

    public BoundStatement UpdateIsEmailVerified(long userId, bool status)
    {
        return _updateEmailVerified.Bind(status, userId);
    }

    public BoundStatement UpdateTotpMfaStatus(User user)
    {
        return _updateMfaStatusKey.Bind(user.TOTPKey, user.TwoFactorAuthentication, user.Id);
    }

    public BoundStatement UpdateAvatar(long userId, string? image)
    {
        return _updateAvatar.Bind(image, userId);
    }
}
