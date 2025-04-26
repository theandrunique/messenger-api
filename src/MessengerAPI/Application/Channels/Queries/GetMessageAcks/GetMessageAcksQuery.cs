using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessageAcks;

public record GetMessageAcksQuery(long ChannelId, long MessageId) : IRequest<ErrorOr<GetMessageAcksQueryResult>>;
