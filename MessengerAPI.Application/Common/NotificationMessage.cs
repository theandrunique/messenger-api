using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common;

public class NotificationMessage
{
    public List<UserId> RecipientIds { get; set; }
    public string JsonData { get; set; }

    public NotificationMessage(List<UserId> recipientIds, string jsonData)
    {
        RecipientIds = recipientIds;
        JsonData = jsonData;
    }
}
