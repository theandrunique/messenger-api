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
    public string? LastReadMessageId { get; init; }
    public string? MaxReadMessageId { get; init; }
    public MessageInfoSchema? LastMessage { get; init; }
    public List<UserPublicSchema> Members { get; init; }

    private ChannelSchema(Channel channel, long? userId = null)
    {
        Id = channel.Id.ToString();
        OwnerId = channel.OwnerId?.ToString();
        Title = channel.Title;
        Image = channel.Image;
        Type = channel.Type;
        if (channel.LastMessage.HasValue)
        {
            var author = channel.AllMembers
                .First(m => m.UserId == channel.LastMessage.Value.AuthorId);

            ChannelMemberInfo? targetUser = null;
            if (channel.LastMessage.Value.TargetUserId.HasValue)
            {
                targetUser = channel.AllMembers
                    .FirstOrDefault(m => m.UserId == channel.LastMessage.Value.TargetUserId);
            }

            LastMessage = MessageInfoSchema.From(
                channel.LastMessage.Value,
                UserPublicSchema.From(author),
                targetUser == null ? null : UserPublicSchema.From(targetUser));
        }
        Members = channel.ActiveMembers.Where(m => !m.IsLeave).Select(UserPublicSchema.From).ToList();
        if (userId != null)
        {
            var currentMember = channel.ActiveMembers.First(m => m.UserId == userId);
            LastReadMessageId = currentMember.LastReadMessageId.ToString();

            MaxReadMessageId = channel.ActiveMembers
                .Where(m => m.UserId != userId)
                .Select(m => (long?)m.LastReadMessageId)
                .Max()?.ToString();
        }
    }

    public static ChannelSchema From(Channel channel, long? userId = null) => new(channel, userId);
    public static List<ChannelSchema> From(IEnumerable<Channel> channels, long? userId = null)
        => channels.Select(c => From(c, userId)).ToList();
}
