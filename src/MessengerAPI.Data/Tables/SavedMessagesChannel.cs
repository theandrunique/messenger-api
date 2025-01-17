using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Tables;

internal class SavedMessagesChannel
{
    public long UserId { get; set; }
    public long ChannelId { get; set; }

    public static SavedMessagesChannel FromChannel(Channel channel)
    {
        return new SavedMessagesChannel()
        {
            ChannelId = channel.Id,
            UserId = channel.Members.First().Id,
        };
    }
}
