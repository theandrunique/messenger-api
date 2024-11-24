namespace MessengerAPI.Repositories.Models;

internal class ChannelPrivate
{
    public Guid Id { get; set; }
    public Guid UserId1 { get; set; }
    public Guid UserId2 { get; set; }
    public long? LastMessageId { get; set; }
}
