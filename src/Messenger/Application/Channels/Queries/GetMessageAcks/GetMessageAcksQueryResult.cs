using Messenger.Contracts.Common;
using Messenger.Domain.ValueObjects;

namespace Messenger.Application.Channels.Queries.GetMessageAcks;

public record GetMessageAcksQueryResult
{
    public record MessageAck(UserPublicSchema user, DateTimeOffset Timestamp);

    public List<MessageAck> Acks { get; init; } = new();

    public GetMessageAcksQueryResult(Dictionary<ChannelMemberInfo, DateTimeOffset> acks)
    {
        Acks = acks.Select(x => new MessageAck(UserPublicSchema.From(x.Key), x.Value)).ToList();
    }
}
