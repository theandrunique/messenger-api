namespace MessengerAPI.Domain.Models.Entities;

public class Session
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
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

    public static Session Create(long id, long userId, string deviceName, string clientName, string location)
    {
        var createdAt = DateTime.UtcNow;
        var session = new Session
        {
            Id = id,
            UserId = userId,
            TokenId = Guid.NewGuid(),
            DeviceName = deviceName,
            ClientName = clientName,
            Location = location,
            LastUsedAt = createdAt,
            CreatedAt = createdAt,
        };
        return session;
    }
}
