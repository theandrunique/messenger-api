using MessengerAPI.Domain.Entities.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public DateTime UsernameUpdatedAt { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime PasswordUpdatedAt { get; private set; }
    public string TerminateSessions { get; private set; } = "";
    public string? Bio { get; private set; }
    public string GlobalName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public byte[] Key { get; private set; }
    public bool TwoFactorAuthentication { get; private set; }
    public string Email { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTime EmailUpdatedAt { get; private set; }

    public IReadOnlyList<Image> Images => _images.ToList();
    private readonly List<Image> _images = new();

    public static User Create(
        string username,
        string email,
        string passwordHash,
        string globalName,
        byte[] key)
    {
        var dateOfCreation = DateTime.UtcNow;

        User user = new User
        {
            Id = Guid.NewGuid(),
            Username = username.ToLower(),
            UsernameUpdatedAt = dateOfCreation,
            PasswordHash = passwordHash,
            PasswordUpdatedAt = dateOfCreation,
            // TerminateSessions = TimeIntervals.Month6,
            GlobalName = globalName,
            IsActive = true,
            CreatedAt = dateOfCreation,
            TwoFactorAuthentication = false,
            Email = email.ToLower(),
            EmailUpdatedAt = dateOfCreation,
            Key = key,
        };

        return user;
    }
}

