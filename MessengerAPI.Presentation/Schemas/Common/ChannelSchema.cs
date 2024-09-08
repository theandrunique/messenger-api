using System.Text.Json.Serialization;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Presentation.Schemas.Common;

public record ChannelSchema
{
    public Guid Id { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? OwnerId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FileData? Image { get; init; }
    public ChannelType Type { get; init; }
    public int? LastMessageId { get; init; }
    public List<UserPublicSchema> Members { get; init; }
    public List<Guid> AdminIds { get; init; }
}
