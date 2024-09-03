namespace MessengerAPI.Domain.Common.Entities;

public class Session
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string DeviceName { get; private set; }
    public string ClientName { get; private set; }
    public string Location { get; private set; }
    public DateTime LastUsedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void UpdateLastUsedAt(DateTime lastUsedAt)
    {
        LastUsedAt = lastUsedAt;
    }
}
