using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.User.Entities;

public class Session
{
    public SessionId Id { get; private set; }
    public UserId UserId { get; private set; }
    public Guid TokenId { get; private set; }
    public string DeviceName { get; private set; }
    public string ClientName { get; private set; }
    public string Location { get; private set; }
    public DateTime LastUsedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void UpdateTokenId()
    {
        LastUsedAt = DateTime.UtcNow;
        TokenId = Guid.NewGuid();
    }

    public static Session CreateNew(string deviceName, string clientName, string location)
    {
        var session = new Session
        {
            Id = new SessionId(Guid.NewGuid()),
            TokenId = Guid.NewGuid(),
            DeviceName = deviceName,
            ClientName = clientName,
            Location = location,
            LastUsedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };
        return session;
    }
}
