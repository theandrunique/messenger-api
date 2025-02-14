using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.CreateAttachment;

class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, ErrorOr<List<CloudAttachmentSchema>>>
{
    private AttachmentService _attachmentService;

    public CreateAttachmentCommandHandler(AttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public async Task<ErrorOr<List<CloudAttachmentSchema>>> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        List<CloudAttachmentSchema> attachments = new();

        foreach (var file in request.Files)
        {
            var uploadUrlDto = await _attachmentService.GenerateUploadUrlAsync(
                    file.FileSize,
                    request.ChannelId,
                    file.Filename);

            attachments.Add(new CloudAttachmentSchema(
                id: file.Id,
                uploadFilename: uploadUrlDto.UploadFilename,
                uploadUrl: uploadUrlDto.UploadUrl
            ));
        }

        return attachments;
    }
}
