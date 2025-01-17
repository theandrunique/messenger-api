using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery(
    long Sub
) : IRequest<ErrorOr<List<ChannelSchema>>>;
