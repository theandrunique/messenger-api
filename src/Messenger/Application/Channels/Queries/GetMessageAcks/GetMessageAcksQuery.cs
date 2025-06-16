using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetMessageAcks;

public record GetMessageAcksQuery(long ChannelId, long MessageId) : IRequest<ErrorOr<GetMessageAcksQueryResult>>;
