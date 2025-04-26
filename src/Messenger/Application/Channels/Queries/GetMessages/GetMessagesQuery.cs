using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetMessages;

public record GetMessagesQuery(
    long ChannelId,
    long Before,
    int Limit
) : IRequest<ErrorOr<List<MessageSchema>>>;
