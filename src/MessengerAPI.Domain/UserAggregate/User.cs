using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.Events;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.UserAggregate;

public class User : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<Email> _emails = new();
    private readonly List<Phone> _phones = new();
    private readonly List<ProfilePhoto> _profilePhotos = new();
    private readonly List<Session> _sessions = new();
    private readonly List<Channel> _channels = new();

    /// <summary>
    /// List of user's emails <see cref="Email"/>
    /// </summary>
    public IReadOnlyList<Email> Emails => _emails.ToList();
    /// <summary>
    /// List of user's phones <see cref="Phone"/>
    /// </summary>
    public IReadOnlyList<Phone> Phones => _phones.ToList();
    /// <summary>
    /// List of user's profile photos <see cref="ProfilePhoto"/>
    /// </summary>
    public IReadOnlyList<ProfilePhoto> ProfilePhotos => _profilePhotos.ToList();
    /// <summary>
    /// List of user's sessions <see cref="Session"/>
    /// </summary>
    public IReadOnlyList<Session> Sessions => _sessions.ToList();
    /// <summary>
    /// List of user's channels <see cref="Channel"/>
    /// </summary>
    public IReadOnlyCollection<Channel> Channels => _channels.ToList();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    /// <summary>
    /// User id
    /// </summary>
    public UserId Id { get; private set; }
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

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="passwordHash">Password hash</param>
    /// <param name="globalName">Global name</param>
    /// <returns><see cref="User"/></returns>
    public static User Create(
        string username,
        string passwordHash,
        string globalName)
    {
        User user = new User
        {
            Id = new UserId(Guid.NewGuid()),
            Username = username,
            UsernameUpdatedAt = DateTime.UtcNow,
            PasswordHash = passwordHash,
            PasswordUpdatedAt = DateTime.UtcNow,
            TerminateSessions = TimeIntervals.Month6,
            GlobalName = globalName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            TwoFactorAuthentication = false
        };
        user._domainEvents.Add(new NewUserCreated(user));

        return user;
    }
    private User() { }

    /// <summary>
    /// Add a new email
    /// </summary>
    /// <param name="email"><see cref="Email"/></param>
    public void AddEmail(Email email)
    {
        _emails.Add(email);
    }

    /// <summary>
    /// Create a new session
    /// </summary>
    /// <param name="deviceName">Device name</param>
    /// <param name="clientName">Client name</param>
    /// <param name="location">Location</param>
    /// <returns><see cref="Session"/></returns>
    public Session CreateSession(string deviceName, string clientName, string location)
    {
        var session = Session.CreateNew(deviceName, clientName, location);
        _sessions.Add(session);
        return session;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
