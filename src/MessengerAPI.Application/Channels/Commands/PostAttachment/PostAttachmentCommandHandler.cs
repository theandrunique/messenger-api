using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

class PostAttachmentCommandHandler : IRequestHandler<PostAttachmentCommand, ErrorOr<List<CloudAttachmentSchema>>>
{
    private AttachmentService _attachmentService;

    public PostAttachmentCommandHandler(AttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public async Task<ErrorOr<List<CloudAttachmentSchema>>> Handle(PostAttachmentCommand request, CancellationToken cancellationToken)
    {
        List<CloudAttachmentSchema> attachments = new();

        foreach (var file in request.Files)
        {
            var uploadUrlDto = await _attachmentService.GenerateUploadUrlAsync(
                    file.Size,
                    request.ChannelId,
                    file.Filename);

            attachments.Add(new CloudAttachmentSchema
            {
                UploadFilename = uploadUrlDto.UploadFilename,
                UploadUrl = uploadUrlDto.UploadUrl,
            });
        }

        return attachments;
    }
}
