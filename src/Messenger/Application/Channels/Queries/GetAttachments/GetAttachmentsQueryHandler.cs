using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Data.Interfaces.Channels;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetAttachments;

public class GetAttachmentsQueryHandler : IRequestHandler<GetAttachmentsQuery, ErrorOr<List<AttachmentSchema>>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly AttachmentService _attachmentService;
    private readonly IClientInfoProvider _clientInfo;

    public GetAttachmentsQueryHandler(
        IChannelRepository channelRepository,
        IAttachmentRepository attachmentRepository,
        AttachmentService attachmentService,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _attachmentRepository = attachmentRepository;
        _attachmentService = attachmentService;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<List<AttachmentSchema>>> Handle(GetAttachmentsQuery request, CancellationToken cancellationToken)
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

        var attachments = await _attachmentRepository
            .GetChannelAttachmentsAsync(request.ChannelId, request.Before, request.Limit);

        await _attachmentService.UpdateUrlsAsync(channel.Id, attachments);

        return AttachmentSchema.From(attachments);
    }
}
