using MediatR;
using MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessagesPrivateChannel;

public class GetMessagesPrivateChannelQueryHandler
    : IRequestHandler<GetMessagesPrivateChannelQuery, ErrorOr<List<MessageSchema>>>
{
    private readonly IMediator _mediator;
    private readonly IMessageRepository _messageRepository;

    public GetMessagesPrivateChannelQueryHandler(IMediator mediator, IMessageRepository messageRepository)
    {
        _mediator = mediator;
        _messageRepository = messageRepository;
    }

    public async Task<ErrorOr<List<MessageSchema>>> Handle(GetMessagesPrivateChannelQuery request, CancellationToken cancellationToken)
    {
        var getPrivateChannelResult = await _mediator.Send(
            new GetOrCreatePrivateChannelCommand(request.UserId),
            cancellationToken
        );

        if (getPrivateChannelResult.IsError)
        {
            return getPrivateChannelResult.Error;
        }

        var channel = getPrivateChannelResult.Value;
        var channelId = long.Parse(channel.Id);

        var messages = await _messageRepository.GetMessagesAsync(channelId, request.Before, request.Limit);

        return MessageSchema.From(messages);
    }
}
