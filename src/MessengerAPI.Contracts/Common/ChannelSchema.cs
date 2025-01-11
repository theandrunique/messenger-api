using System.Threading.Channels;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Channel schema for response <see cref="Channel"/> 
/// </summary>
public record ChannelSchema
{
    public Guid Id { get; init; }
    public Guid? OwnerId { get; init; }
    public string? Title { get; init; }
    public string? Image { get; init; }
    public ChannelType Type { get; init; }
    public DateTime? LastMessageAt { get; init; }
    public MessageInfo? LastMessage { get; init; }
    public List<ChannelMemberInfo> Members { get; init; }
}
