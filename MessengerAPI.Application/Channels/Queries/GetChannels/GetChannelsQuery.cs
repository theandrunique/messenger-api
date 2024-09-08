using ErrorOr;
using MediatR;
using MessengerAPI.Domain.Channel;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery(
    UserId Sub
) : IRequest<ErrorOr<List<Channel>>>;
