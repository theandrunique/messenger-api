using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Tables;

internal class ChannelById
{
    public Guid ChannelId { get; set; }
    public Guid? OwnerId { get; set; }
    public string? Title { get; set; }
    public Image? Image { get; set; }
    public ChannelType ChannelType { get; set; }
    public DateTime? LastMessageAt { get; private set; }
    public MessageInfo? LastMessage { get; private set; }

    public static ChannelById FromChannel(Channel channel) => new ChannelById
    {
        ChannelId = channel.Id,
        OwnerId = channel.OwnerId,
        Title = channel.Title,
        Image = channel.Image,
        ChannelType = channel.Type,
        LastMessageAt = channel.LastMessageAt,
        LastMessage = channel.LastMessage,
    };

    public Channel ToChannel()
        => new Channel(
            ChannelId,
            OwnerId,
            Title,
            Image,
            ChannelType,
            LastMessageAt,
            LastMessage);
}
