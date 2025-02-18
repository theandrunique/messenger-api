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

    public Task<ErrorOr<List<CloudAttachmentSchema>>> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        List<CloudAttachmentSchema> attachments = new();

        foreach (var file in request.Files)
        {
            var uploadUrlDto = _attachmentService.GenerateUploadUrl(
                size: file.FileSize,
                channelId: request.ChannelId,
                filename: file.Filename);

            attachments.Add(new CloudAttachmentSchema(
                id: file.Id,
                uploadFilename: uploadUrlDto.UploadFilename,
                uploadUrl: uploadUrlDto.UploadUrl
            ));
        }

        return Task.FromResult(new ErrorOr<List<CloudAttachmentSchema>>(attachments));
    }
}
