namespace MessengerAPI.Repositories.Models;

internal class ChannelSavedMessages
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public long? LastMessageId { get; set; }
}
