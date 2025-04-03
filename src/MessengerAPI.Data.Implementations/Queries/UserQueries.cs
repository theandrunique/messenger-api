using Cassandra;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Implementations.Queries;

public class UserQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByIds;
    private readonly PreparedStatement _selectByEmail;
    private readonly PreparedStatement _selectByUsername;
    private readonly PreparedStatement _updateEmailInfo;
    private readonly PreparedStatement _updateMfaStatusKey;
    private readonly PreparedStatement _updateEmailVerified;
    private readonly PreparedStatement _updateAvatar;

    public UserQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO users (
                id,
                bio,
                timestamp,
                terminatesessions,
                email,
                emailupdatedtimestamp,
                globalname,
                isactive,
                isemailverified,
                totpkey,
                passwordhash,
                passwordupdatedtimestamp,
                twofactorauthentication,
                username,
                usernameupdatedtimestamp,
                image) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """);

        _selectById = session.Prepare("SELECT * FROM users WHERE id = ?");

        _selectByIds = session.Prepare("SELECT * FROM users WHERE id IN ?");

        _selectByEmail = session.Prepare("SELECT * FROM users WHERE email = ?");

        _selectByUsername = session.Prepare("SELECT * FROM users WHERE username = ?");

        _updateEmailInfo = session.Prepare("UPDATE users SET email = ?, emailupdatedtimestamp = ?, isemailverified = ? WHERE id = ?");

        _updateMfaStatusKey = session.Prepare("UPDATE users SET totpkey = ?, twofactorauthentication = ? WHERE id = ?");

        _updateEmailVerified = session.Prepare("UPDATE users SET isemailverified = ? WHERE id = ?");

        _updateAvatar = session.Prepare("UPDATE users SET image = ? WHERE id = ?");
    }

    public BoundStatement Insert(User user)
    {
        return _insert.Bind(
            user.Id,
            user.Bio,
            user.Timestamp,
            (int)user.TerminateSessions,
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

    public BoundStatement SelectByEmail(string email)
    {
        return _selectByEmail.Bind(email);
    }

    public BoundStatement SelectByUsername(string username)
    {
        return _selectByUsername.Bind(username);
    }

    public BoundStatement UpdateEmail(
        long userId,
        string email,
        DateTimeOffset emailUpdatedTimestamp,
        bool isEmailVerified)
    {
        return _updateEmailInfo.Bind(email, emailUpdatedTimestamp, isEmailVerified, userId);
    }

    public BoundStatement UpdateEmailVerified(long userId, bool status)
    {
        return _updateEmailVerified.Bind(status, userId);
    }

    public BoundStatement UpdateMfaStatus(User user)
    {
        return _updateMfaStatusKey.Bind(user.TOTPKey, user.TwoFactorAuthentication, user.Id);
    }

    public BoundStatement UpdateAvatar(long userId, string? image)
    {
        return _updateAvatar.Bind(image, userId);
    }
}
