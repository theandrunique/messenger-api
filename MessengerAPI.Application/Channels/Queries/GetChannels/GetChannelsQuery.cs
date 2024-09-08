using ErrorOr;
using MediatR;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery(
    UserId Sub
) : IRequest<ErrorOr<List<Channel>>>;
