using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.Events;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.UserAggregate;

public class User : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<Image> _images = new();

    /// <summary>
    /// List of user's images <see cref="Image"/>
    /// </summary>
    public IReadOnlyList<Image> Images => _images.ToList();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();

    /// <summary>
    /// User id
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; private set; }
    /// <summary>
    /// Last time username was updated
    /// </summary>
    public DateTime UsernameUpdatedAt { get; private set; }
    /// <summary>
    /// Password hash
    /// </summary>
    public string PasswordHash { get; private set; }
    /// <summary>
    /// Last time password was updated
    /// </summary>
    public DateTime PasswordUpdatedAt { get; private set; }
    /// <summary>
    /// Lifetime of sessions
    /// </summary>
    public TimeIntervals TerminateSessions { get; private set; }
    /// <summary>
    /// User bio
    /// </summary>
    public string? Bio { get; private set; }
    /// <summary>
    /// User global name
    /// </summary>
    public string GlobalName { get; private set; }
    /// <summary>
    /// Is user active
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Date when user was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// User secret key to generate TOTP
    /// </summary>
    public string? Key { get; private set; }
    /// <summary>
    /// Is two factor authentication enabled
    /// </summary>
    public bool TwoFactorAuthentication { get; private set; }

    public string Email { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public DateTime EmailUpdatedAt { get; private set; }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="email">Email</param>
    /// <param name="passwordHash">Password hash</param>
    /// <param name="globalName">Global name</param>
    /// <returns><see cref="User"/></returns>
    public static User Create(
        string username,
        string email,
        string passwordHash,
        string globalName)
    {
        var dateOfCreation = DateTime.UtcNow;

        User user = new User
        {
            Id = Guid.NewGuid(),
            Username = username.ToLower(),
            UsernameUpdatedAt = dateOfCreation,
            PasswordHash = passwordHash,
            PasswordUpdatedAt = dateOfCreation,
            TerminateSessions = TimeIntervals.Month6,
            GlobalName = globalName,
            IsActive = true,
            CreatedAt = dateOfCreation,
            TwoFactorAuthentication = false,
            Email = email.ToLower(),
            EmailUpdatedAt = dateOfCreation
        };

        user._domainEvents.Add(new NewUserCreated(user));

        return user;
    }

    private User() { }

    /// <summary>
    /// Create a new session
    /// </summary>
    /// <param name="deviceName">Device name</param>
    /// <param name="clientName">Client name</param>
    /// <param name="location">Location</param>
    /// <returns><see cref="Session"/></returns>
    public Session CreateSession(string deviceName, string clientName, string location)
    {
        return Session.CreateNew(this.Id, deviceName, clientName, location);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

