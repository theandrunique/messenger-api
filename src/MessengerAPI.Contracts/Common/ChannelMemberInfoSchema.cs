using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Contracts.Common;

public record ChannelMemberInfoSchema
{
    public string UserId { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public string? Image { get; init; }

    private ChannelMemberInfoSchema(ChannelMemberInfo memberInfo)
    {
        UserId = memberInfo.UserId.ToString();
        Username = memberInfo.Username;
        GlobalName = memberInfo.GlobalName;
        Image = memberInfo.Image?.Key;
    }

    public static ChannelMemberInfoSchema From(ChannelMemberInfo memberInfo) => new(memberInfo);
}
