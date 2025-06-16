using Messenger.Domain.Auth.ValueObjects;

namespace Messenger.Domain.Auth;

public class User
{
    public long Id { get; private set; }
    public string Username { get; private set; }
    public DateTimeOffset UsernameUpdatedTimestamp { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTimeOffset PasswordUpdatedTimestamp { get; private set; }
    public SessionLifetime SessionsLifetime { get; private set; }
    public string? Bio { get; private set; }
    public string GlobalName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public byte[]? TOTPKey { get; private set; }
    public bool TwoFactorAuthentication { get; private set; }
    public string Email { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTimeOffset EmailUpdatedTimestamp { get; private set; }
    public string? Image { get; private set; }

    public User(
        long id,
        string username,
        string email,
        string passwordHash,
        string globalName)
    {
        var timestamp = DateTimeOffset.UtcNow;

        Id = id;
        Username = username.ToLower();
        UsernameUpdatedTimestamp = timestamp;
        PasswordHash = passwordHash;
        PasswordUpdatedTimestamp = timestamp;
        SessionsLifetime = SessionLifetime.MONTH6;
        GlobalName = globalName;
        IsActive = true;
        Timestamp = timestamp;
        TwoFactorAuthentication = false;
        Email = email.ToLower();
        EmailUpdatedTimestamp = timestamp;
    }

    public User(
        long id,
        string username,
        DateTimeOffset usernameUpdatedTimestamp,
        string passwordHash,
        DateTimeOffset passwordUpdatedTimestamp,
        SessionLifetime sessionLifetime,
        string? bio,
        string globalName,
        bool isActive,
        DateTimeOffset timestamp,
        byte[] totpkey,
        bool twoFactorAuthentication,
        string email,
        bool isEmailVerified,
        DateTimeOffset emailUpdatedTimestamp,
        string? image)
    {
        Id = id;
        Username = username;
        UsernameUpdatedTimestamp = usernameUpdatedTimestamp;
        PasswordHash = passwordHash;
        PasswordUpdatedTimestamp = passwordUpdatedTimestamp;
        SessionsLifetime = sessionLifetime;
        Bio = bio;
        GlobalName = globalName;
        IsActive = isActive;
        Timestamp = timestamp;
        TOTPKey = totpkey;
        TwoFactorAuthentication = twoFactorAuthentication;
        Email = email;
        IsEmailVerified = isEmailVerified;
        EmailUpdatedTimestamp = emailUpdatedTimestamp;
        Image = image;
    }

    public void EnableTotp2FA(byte[] totpkey)
    {
        if (TwoFactorAuthentication)
        {
            throw new InvalidOperationException("TOTP 2FA is already enabled");
        }

        TOTPKey = totpkey;
        TwoFactorAuthentication = true;
    }

    public void DisableTotp2FA()
    {
        if (!TwoFactorAuthentication)
        {
            throw new InvalidOperationException("TOTP 2FA is already disabled");
        }

        TOTPKey = null;
        TwoFactorAuthentication = false;
    }

    public void SetNewPassword(string passwordHash) => PasswordHash = passwordHash;

    public void UpdateAvatar(string avatarHash)
    {
        Image = avatarHash;
    }

    public void RemoveAvatar() => Image = null;
}
