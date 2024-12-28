using MessengerAPI.Application.Channels.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Files;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Infrastructure.Channels;

public class AttachmentService : IAttachmentService
{
    private readonly IFileStorageService _fileStorage;

    public AttachmentService(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public async Task<(string, string)> GenerateUploadUrlAsync(long size, Guid channelId, string filename)
    {
        var uploadFilename = GenerateUploadFilename(filename, channelId);

        var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForUploadAsync(
            uploadFilename,
            DateTime.UtcNow.AddDays(1),
            size
        );
        return (uploadFilename, preSignedUrl);
    }

    public async Task<ErrorOr<Attachment>> ValidateAndCreateAttachmentsAsync(
        string uploadedFilename,
        string filename,
        Guid channelId,
        CancellationToken cancellationToken)
    {
        var objectMetadata = await _fileStorage.GetObjectMetadataAsync(uploadedFilename, cancellationToken);
        if (objectMetadata is null)
        {
            return Error.File.NotFound(uploadedFilename);
        }

        var preSignedUrlExpiresAt = DateTime.UtcNow.AddDays(7);

        var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForDownloadOrNullAsync(
            uploadedFilename,
            preSignedUrlExpiresAt);

        return new Attachment(
            channelId,
            filename,
            uploadedFilename,
            objectMetadata.ContentType,
            objectMetadata.ObjectSize,
            preSignedUrl,
            preSignedUrlExpiresAt
        );
    }

    public async Task<ErrorOr<bool>> DeleteAttachmentAsync(string uploadedFilename, CancellationToken cancellationToken)
    {
        var result = await _fileStorage.DeleteObjectAsync(uploadedFilename, cancellationToken);
        if (!result)
        {
            return Error.File.NotFound(uploadedFilename);
        }
        return true;
    }

    private string GenerateUploadFilename(string filename, Guid channelId)
    {
        return $"attachment/{channelId}/{Guid.NewGuid()}/{filename}";
    }
}

