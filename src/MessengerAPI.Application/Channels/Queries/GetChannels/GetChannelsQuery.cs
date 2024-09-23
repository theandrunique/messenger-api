using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

/// <summary>
/// Get user's channels query
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
public record GetChannelsQuery(
    UserId Sub
) : IRequest<ErrorOr<List<ChannelSchema>>>;
