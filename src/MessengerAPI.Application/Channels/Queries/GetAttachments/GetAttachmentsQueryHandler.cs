using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetAttachments;

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
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var attachments = await _attachmentRepository
            .GetChannelAttachmentsAsync(request.ChannelId, request.Before, request.Limit);

        await _attachmentService.UpdateUrlsAsync(channel.Id, attachments);
        
        return AttachmentSchema.From(attachments);
    }
}
