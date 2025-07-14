using MediatR;
using Messenger.Contracts.Common;
using Messenger.Domain.Data.Messages;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetLastMessages;

public class GetLastMessagesQueryHandler : IRequestHandler<GetLastMessagesQuery, ErrorOr<Dictionary<string, MessageSchema?>>>
{
    private readonly IMessageRepository _messagesRepository;

    public GetLastMessagesQueryHandler(IMessageRepository messagesRepository)
    {
        _messagesRepository = messagesRepository;
    }

    public async Task<ErrorOr<Dictionary<string, MessageSchema?>>> Handle(GetLastMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messagesRepository.GetLastMessages(request.ChannelIds);

        Dictionary<string, MessageSchema?> result = new();

        foreach (var m in messages)
        {
            result.Add(m.ChannelId.ToString(), MessageSchema.From(m));
        }

        return result;
    }
}
