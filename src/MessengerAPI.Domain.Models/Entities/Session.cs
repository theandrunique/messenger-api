namespace MessengerAPI.Domain.Models.Entities;

public class Session
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public Guid TokenId { get; private set; }
    public string DeviceName { get; private set; }
    public string ClientName { get; private set; }
    public string Location { get; private set; }
    public DateTimeOffset LastUsedTimestamp { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

    public Session(long id, long userId, string deviceName, string clientName, string location)
    {
        var timestamp = DateTimeOffset.UtcNow;
        Id = id;
        UserId = userId;
        TokenId = Guid.NewGuid();
        DeviceName = deviceName;
        ClientName = clientName;
        Location = location;
        LastUsedTimestamp = timestamp;
        Timestamp = timestamp;
    }

    public Session(
        long id,
        long userId,
        Guid tokenId,
        string deviceName,
        string clientName,
        string location,
        DateTimeOffset lastUsedTimestamp,
        DateTimeOffset timestamp)
    {
        Id = id;
        UserId = userId;
        TokenId = tokenId;
        DeviceName = deviceName;
        ClientName = clientName;
        Location = location;
        LastUsedTimestamp = lastUsedTimestamp;
        Timestamp = timestamp;
    }

    public Session() { }

    public void UpdateTokenId()
    {
        LastUsedTimestamp = DateTimeOffset.UtcNow;
        TokenId = Guid.NewGuid();
    }
}
