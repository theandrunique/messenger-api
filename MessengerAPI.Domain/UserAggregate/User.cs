using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.UserAggregate;

public class User
{
    private readonly List<Email> _emails = new();
    private readonly List<Phone> _phones = new();
    private readonly List<ProfilePhoto> _profilePhotos = new();
    private readonly List<Session> _sessions = new();
    private readonly List<Channel> _channels = new();

    public IReadOnlyList<Email> Emails => _emails.ToList();
    public IReadOnlyList<Phone> Phones => _phones.ToList();
    public IReadOnlyList<ProfilePhoto> ProfilePhotos => _profilePhotos.ToList();
    public IReadOnlyList<Session> Sessions => _sessions.ToList();
    public IReadOnlyCollection<Channel> Channels => _channels.ToList();

    public UserId Id { get; private set; }
    public string Username { get; private set; }
    public DateTime UsernameUpdatedAt { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime PasswordUpdatedAt { get; private set; }
    public TimeIntervals TerminateSessions { get; private set; }
    public string? Bio { get; private set; }
    public string GlobalName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? Key { get; private set; }
    public bool TwoFactorAuthentication { get; private set; }

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

        return user;
    }
    public User() { }

    public void AddEmail(Email email)
    {
        _emails.Add(email);
    }

    public Session CreateSession(string deviceName, string clientName, string location)
    {
        var session = Session.CreateNew(deviceName, clientName, location);
        _sessions.Add(session);
        return session;
    }
}
