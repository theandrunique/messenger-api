namespace MessengerAPI.Infrastructure.Common.WebSockets;

public class NotificationMessage
{
    /// <summary>
    /// List of recipient ids
    /// </summary>
    public List<Guid> RecipientIds { get; set; }
    /// <summary>
    /// Json data to send
    /// </summary>
    public string JsonData { get; set; }

    public NotificationMessage(List<Guid> recipientIds, string jsonData)
    {
        RecipientIds = recipientIds;
        JsonData = jsonData;
    }
}
