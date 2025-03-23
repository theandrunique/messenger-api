using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetChannel;

public record GetChannelQuery(long ChannelId) : IRequest<ErrorOr<ChannelSchema>>;
