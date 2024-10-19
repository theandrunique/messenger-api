using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Files;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

class PostAttachmentCommandHandler : IRequestHandler<PostAttachmentCommand, ErrorOr<List<CloudAttachmentSchema>>>
{
    private readonly IFileStorageService _fileStorage;

    public PostAttachmentCommandHandler(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
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
            var key = GenerateStoragekey(file.Filename);

            var presignedUrl = await _fileStorage.GeneratePreSignedUrlForUploadAsync(
                key,
                DateTime.UtcNow.AddDays(1),
                file.Size
            );

            attachments.Add(new CloudAttachmentSchema
            {
                UploadFilename = key,
                UploadUrl = presignedUrl,
            });
        }

        return attachments;
    }
}
