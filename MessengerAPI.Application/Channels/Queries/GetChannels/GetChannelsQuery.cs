using ErrorOr;
using MediatR;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery(
    UserId Sub
) : IRequest<ErrorOr<GetChannelsResult>>;
