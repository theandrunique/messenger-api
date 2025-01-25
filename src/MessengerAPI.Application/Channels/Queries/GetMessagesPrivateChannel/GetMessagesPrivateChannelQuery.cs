using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessagesPrivateChannel;

public record GetMessagesPrivateChannelQuery(
    long Sub,
    long UserId,
    long Before,
    int Limit
) : IRequest<ErrorOr<List<MessageSchema>>>;
