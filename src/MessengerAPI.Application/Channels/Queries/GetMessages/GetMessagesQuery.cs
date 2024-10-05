using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

/// <summary>
/// Get messages from requested channel
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
/// <param name="ChannelId"></param>
/// <param name="Offset">offset</param>
/// <param name="Limit">limit</param>
public record GetMessagesQuery(
    Guid Sub,
    Guid ChannelId,
    int Offset,
    int Limit) : IRequest<ErrorOr<List<MessageSchema>>>;
