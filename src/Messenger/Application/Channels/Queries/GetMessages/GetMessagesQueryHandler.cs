using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Data.Interfaces.Channels;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, ErrorOr<List<MessageSchema>>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly AttachmentService _attachmentService;

    public GetMessagesQueryHandler(
        IChannelRepository channelRepository,
        IMessageRepository messageRepository,
        IClientInfoProvider clientInfo,
        AttachmentService attachmentService)
    {
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _clientInfo = clientInfo;
        _attachmentService = attachmentService;
    }

    public async Task<ErrorOr<List<MessageSchema>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return Errors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Errors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var messages = await _messageRepository
            .GetMessagesAsync(request.ChannelId, request.Before, request.Limit);

        await _attachmentService.UpdateUrlsAsync(channel.Id, messages.SelectMany(m => m.Attachments));

        return MessageSchema.From(messages);
    }
}
