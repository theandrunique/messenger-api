using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

public record GetMessagesQuery(
    long ChannelId,
    long Before,
    int Limit
) : IRequest<ErrorOr<List<MessageSchema>>>;
