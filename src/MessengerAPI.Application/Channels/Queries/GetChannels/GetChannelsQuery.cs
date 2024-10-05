using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

/// <summary>
/// Get user's channels query
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
public record GetChannelsQuery(
    Guid Sub
) : IRequest<ErrorOr<List<ChannelSchema>>>;
