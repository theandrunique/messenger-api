using MediatR;
using MessengerAPI.Application.Channels.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Files;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

class PostAttachmentCommandHandler : IRequestHandler<PostAttachmentCommand, ErrorOr<List<CloudAttachmentSchema>>>
{
    private readonly IFileStorageService _fileStorage;
    private IAttachmentService _attachmentService;

    public PostAttachmentCommandHandler(IFileStorageService fileStorage, IAttachmentService attachmentService)
    {
        _fileStorage = fileStorage;
        _attachmentService = attachmentService;
    }

    private string GenerateStoragekey(string filename)
    {
        return $"{Guid.NewGuid()}/{filename}";
    }

    public async Task<ErrorOr<List<CloudAttachmentSchema>>> Handle(PostAttachmentCommand request, CancellationToken cancellationToken)
    {
        List<CloudAttachmentSchema> attachments = new();

        foreach (var file in request.Files)
        {
            (string uploadFilename, string presignedUrl) = await _attachmentService.GenerateUploadUrlAsync(
                    file.Size,
                    request.ChannelId,
                    file.Filename);

            attachments.Add(new CloudAttachmentSchema
            {
                UploadFilename = uploadFilename,
                UploadUrl = presignedUrl,
            });
        }

        return attachments;
    }
}
