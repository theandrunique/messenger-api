using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Channel schema for response <see cref="Channel"/> 
/// </summary>
public record ChannelSchema
{
    public string Id { get; init; }
    public string? OwnerId { get; init; }
    public string? Title { get; init; }
    public string? Image { get; init; }
    public ChannelType Type { get; init; }
    public DateTimeOffset? LastMessageTimestamp { get; init; }
    public MessageInfoSchema? LastMessage { get; init; }
    public List<ChannelMemberInfoSchema> Members { get; init; }

    private ChannelSchema(Channel channel)
    {
        Id = channel.Id.ToString();
        OwnerId = channel.OwnerId?.ToString();
        Title = channel.Title;
        Image = channel.Image?.Key;
        Type = channel.Type;
        LastMessageTimestamp = channel.LastMessageTimestamp;
        if (channel.LastMessage.HasValue)
        {
            LastMessage = MessageInfoSchema.From(channel.LastMessage.Value);
        }
        Members = channel.Members.Select(ChannelMemberInfoSchema.From).ToList();
    }

    public static ChannelSchema From(Channel channel) => new(channel);
    public static List<ChannelSchema> From(IEnumerable<Channel> channels) => channels.Select(From).ToList();
}
