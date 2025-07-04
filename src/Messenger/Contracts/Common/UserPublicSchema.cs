using System.Text.Json.Serialization;
using Messenger.Domain.Entities;
using Messenger.Domain.Users;
using Messenger.Domain.ValueObjects;

namespace Messenger.Contracts.Common;

/// <summary>
/// User public schema for response <see cref="User"/>
/// </summary>
public record UserPublicSchema
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Bio { get; init; }
    public string? Avatar { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? Timestamp { get; init; }

    protected UserPublicSchema(User user)
    {
        Id = user.Id.ToString();
        Username = user.Username;
        GlobalName = user.GlobalName;
        Bio = user.Bio;
        Avatar = user.Image;
        Timestamp = user.Timestamp;
    }

    public static UserPublicSchema From(User user) => new(user);

    private UserPublicSchema(UserIndexModel user)
    {
        Id = user.Id;
        Username = user.Username;
        GlobalName = user.GlobalName;
        Avatar = user.Image;
    }

    public static UserPublicSchema From(UserIndexModel user) => new(user);

    private UserPublicSchema(ChannelMemberInfo member)
    {
        Id = member.UserId.ToString();
        Username = member.Username;
        GlobalName = member.GlobalName;
        Avatar = member.Image;
    }

    public static UserPublicSchema From(ChannelMemberInfo member) => new(member);

    private UserPublicSchema(MessageAuthorInfo authorInfo)
    {
        Id = authorInfo.Id.ToString();
        Username = authorInfo.Username;
        GlobalName = authorInfo.GlobalName;
        Avatar = authorInfo.Image;
    }

    public static UserPublicSchema From(MessageAuthorInfo authorInfo) => new(authorInfo);
}
