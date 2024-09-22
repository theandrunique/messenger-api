using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

public record GetMessagesQuery(
    UserId Sub,
    ChannelId ChannelId,
    int Offset,
    int Limit) : IRequest<ErrorOr<List<MessageSchema>>>;
