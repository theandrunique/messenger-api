using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// User public schema for response <see cref="User"/>
/// </summary>
public record UserPublicSchema
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    protected UserPublicSchema(User user)
    {
        Id = user.Id.ToString();
        Username = user.Username;
        GlobalName = user.GlobalName;
        Bio = user.Bio;
        Image = user.Image?.Key;
        Timestamp = user.Timestamp;
    }

    public static UserPublicSchema From(User user) => new(user);
}
