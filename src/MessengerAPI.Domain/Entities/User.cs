using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Domain.Entities;

public class User
{
    public long Id { get; private set; }
    public string Username { get; private set; }
    public DateTimeOffset UsernameUpdatedTimestamp { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTimeOffset PasswordUpdatedTimestamp { get; private set; }
    public TimeIntervals TerminateSessions { get; private set; }
    public string? Bio { get; private set; }
    public string GlobalName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public byte[]? TOTPKey { get; private set; }
    public bool TwoFactorAuthentication { get; private set; }
    public string Email { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTimeOffset EmailUpdatedTimestamp { get; private set; }
    public Image? Image { get; private set; }

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
        TerminateSessions = TimeIntervals.MONTH6;
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
        TimeIntervals terminateSessions,
        string? bio,
        string globalName,
        bool isActive,
        DateTimeOffset timestamp,
        byte[] totpkey,
        bool twoFactorAuthentication,
        string email,
        bool isEmailVerified,
        DateTimeOffset emailUpdatedTimestamp,
        Image? image)
    {
        Id = id;
        Username = username;
        UsernameUpdatedTimestamp = usernameUpdatedTimestamp;
        PasswordHash = passwordHash;
        PasswordUpdatedTimestamp = passwordUpdatedTimestamp;
        TerminateSessions = terminateSessions;
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

    public void UpdateAvatar(string key)
    {
        Image = new Image { Key = key };
    }

    public void RemoveAvatar() => Image = null;
}
