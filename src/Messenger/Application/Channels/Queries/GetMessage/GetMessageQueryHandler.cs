using MediatR;
using Messenger.ApiErrors;
using Messenger.Contracts.Common;
using Messenger.Data.Interfaces.Channels;

namespace Messenger.Application.Channels.Queries.GetMessage;

public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, ErrorOr<MessageSchema>>
{
    private readonly IMessageRepository _messagesRepository;

    public GetMessageQueryHandler(IMessageRepository messagesRepository)
    {
        _messagesRepository = messagesRepository;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await _messagesRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId);
        if (message == null)
        {
            return Errors.Channel.MessageNotFound(request.MessageId);
        }
        return MessageSchema.From(message);
    }
}
