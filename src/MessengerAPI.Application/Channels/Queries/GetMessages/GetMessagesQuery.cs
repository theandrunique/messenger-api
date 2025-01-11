using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

public record GetMessagesQuery(
    Guid Sub,
    Guid ChannelId,
    Guid Before,
    int Limit) : IRequest<ErrorOr<List<MessageSchema>>>;
