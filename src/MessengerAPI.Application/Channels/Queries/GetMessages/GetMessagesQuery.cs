using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

/// <summary>
/// Get messages from requested channel
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
/// <param name="ChannelId"></param>
/// <param name="Offset">offset</param>
/// <param name="Limit">limit</param>
public record GetMessagesQuery(
    UserId Sub,
    ChannelId ChannelId,
    int Offset,
    int Limit) : IRequest<ErrorOr<List<MessageSchema>>>;
