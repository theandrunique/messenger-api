using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Contracts.Common;

public record MessageAuthorInfoSchema
{
    public string Id { get; private init; }
    public string Username { get; private init; }
    public string GlobalName { get; private init; }
    public string? Image { get; private init; }

    private MessageAuthorInfoSchema(MessageAuthorInfo authorInfo)
    {
        Id = authorInfo.Id.ToString();
        Username = authorInfo.Username;
        GlobalName = authorInfo.GlobalName;
        Image = authorInfo.Image?.Key;
    }

    public static MessageAuthorInfoSchema From(MessageAuthorInfo authorInfo) => new(authorInfo);
}
