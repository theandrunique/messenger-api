using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Infrastructure.Common.WebSockets;

public class NotificationMessage
{
    /// <summary>
    /// List of recipient ids
    /// </summary>
    public List<UserId> RecipientIds { get; set; }
    /// <summary>
    /// Json data to send
    /// </summary>
    public string JsonData { get; set; }

    public NotificationMessage(List<UserId> recipientIds, string jsonData)
    {
        RecipientIds = recipientIds;
        JsonData = jsonData;
    }
}
