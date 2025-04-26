using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetMessageAcks;

public record GetMessageAcksQuery(long ChannelId, long MessageId) : IRequest<ErrorOr<GetMessageAcksQueryResult>>;
