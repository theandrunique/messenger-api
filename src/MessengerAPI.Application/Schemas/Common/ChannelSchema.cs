using MessengerAPI.Domain.ChannelAggregate.ValueObjects;

namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// Channel schema for response
/// </summary>
public record ChannelSchema
{
    /// <summary>
    /// Id of channel
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Owner id, null when chat is private
    /// </summary>
    public Guid? OwnerId { get; init; }
    /// <summary>
    /// Channel title, null when chat is private
    /// </summary>
    public string? Title { get; init; }
    /// <summary>
    /// Channel image, null when chat is private
    /// </summary>
    public string? Image { get; init; }
    /// <summary>
    /// Type of channel
    /// </summary>
    public ChannelType Type { get; init; }
    /// <summary>
    /// Last message id
    /// </summary>
    public long? LastMessageId { get; init; }
    /// <summary>
    /// List of channel members
    /// </summary>
    public List<UserPublicSchema> Members { get; init; }
    /// <summary>
    /// List of admin user ids
    /// </summary>
    public List<Guid> AdminIds { get; init; }
}
