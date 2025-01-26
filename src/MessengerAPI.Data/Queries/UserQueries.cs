using Cassandra;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Queries;

internal class UserQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectById;
    private readonly PreparedStatement _selectByIds;
    private readonly PreparedStatement _selectByEmail;
    private readonly PreparedStatement _selectByUsername;
    private readonly PreparedStatement _updateEmailInfo;
    private readonly PreparedStatement _updateTOTPKey;

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

        _updateEmailInfo = session.Prepare("UPDATE users SET email = ?, emailupdatedtimestamp = ? WHERE id = ?");

        _updateTOTPKey = session.Prepare("UPDATE users SET totpkey = ? WHERE id = ?");
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

    public BoundStatement UpdateEmailInfo(User user)
    {
        return _updateEmailInfo.Bind(user.Email, user.EmailUpdatedTimestamp, user.Id);
    }

    public BoundStatement UpdateTOTPKey(User user)
    {
        return _updateTOTPKey.Bind(user.TOTPKey, user.Id);
    }
}
