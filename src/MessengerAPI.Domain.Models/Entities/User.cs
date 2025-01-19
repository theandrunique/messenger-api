using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

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
    public byte[] TOTPKey { get; private set; }
    public bool TwoFactorAuthentication { get; private set; }
    public string Email { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTimeOffset EmailUpdatedTimestamp { get; private set; }
    public Image? Image { get; set; }

    public static User Create(
        long id,
        string username,
        string email,
        string passwordHash,
        string globalName,
        byte[] totpkey)
    {
        var timestamp = DateTimeOffset.UtcNow;

        User user = new User
        {
            Id = id,
            Username = username.ToLower(),
            UsernameUpdatedTimestamp = timestamp,
            PasswordHash = passwordHash,
            PasswordUpdatedTimestamp = timestamp,
            TerminateSessions = TimeIntervals.Month6,
            GlobalName = globalName,
            IsActive = true,
            Timestamp = timestamp,
            TwoFactorAuthentication = false,
            Email = email.ToLower(),
            EmailUpdatedTimestamp = timestamp,
            TOTPKey = totpkey,
        };

        return user;
    }

    public void SetTOTPKey(byte[] totpkey) => TOTPKey = totpkey;
    public void SetNewPassword(string passwordHash) => PasswordHash = passwordHash;
}

