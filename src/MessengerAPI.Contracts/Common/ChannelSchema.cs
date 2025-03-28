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
    public string? ReadAt { get; init; }
    public string? MaxReadAt { get; init; }
    public DateTimeOffset? LastMessageTimestamp { get; init; }
    public MessageInfoSchema? LastMessage { get; init; }
    public List<UserPublicSchema> Members { get; init; }

    private ChannelSchema(Channel channel, long? userId = null)
    {
        Id = channel.Id.ToString();
        OwnerId = channel.OwnerId?.ToString();
        Title = channel.Title;
        Image = channel.Image;
        Type = channel.Type;
        LastMessageTimestamp = channel.LastMessageTimestamp;
        if (channel.LastMessage.HasValue)
        {
            LastMessage = MessageInfoSchema.From(channel.LastMessage.Value);
        }
        Members = channel.ActiveMembers.Where(m => !m.IsLeave).Select(UserPublicSchema.From).ToList();
        if (userId != null)
        {
            var currentMember = channel.ActiveMembers.First(m => m.UserId == userId);
            ReadAt = currentMember.ReadAt.ToString();

            MaxReadAt = channel.ActiveMembers
                .Where(m => m.UserId != userId)
                .Select(m => (long?)m.ReadAt)
                .Max()?.ToString();
        }
    }

    public static ChannelSchema From(Channel channel, long? userId = null) => new(channel, userId);
    public static List<ChannelSchema> From(IEnumerable<Channel> channels, long? userId = null)
        => channels.Select(c => From(c, userId)).ToList();
}
