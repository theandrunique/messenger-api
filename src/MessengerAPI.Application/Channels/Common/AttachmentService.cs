using MessengerAPI.Application.Common.Interfaces.Files;
using MessengerAPI.Core;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Common;

public class AttachmentService
{
    private readonly IFileStorageService _fileStorage;
    private readonly IIdGenerator _idGenerator;

    public AttachmentService(IFileStorageService fileStorage, IIdGenerator idGenerator)
    {
        _fileStorage = fileStorage;
        _idGenerator = idGenerator;
    }

    public async Task<UploadUrlDto> GenerateUploadUrlAsync(long size, long channelId, string filename)
    {
        var uploadFilename = GenerateUploadFilename(filename, channelId);

        var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForUploadAsync(
            uploadFilename,
            DateTime.UtcNow.AddDays(1),
            size
        );
        return new UploadUrlDto(uploadFilename, preSignedUrl);
    }

    public async Task<ErrorOr<Attachment>> ValidateAndCreateAttachmentsAsync(
        string uploadedFilename,
        string filename,
        long channelId,
        CancellationToken cancellationToken)
    {
        var objectMetadata = await _fileStorage.GetObjectMetadataAsync(uploadedFilename, cancellationToken);
        if (objectMetadata is null)
        {
            return ApiErrors.File.NotFound(uploadedFilename);
        }

        var preSignedUrlExpiresAt = DateTime.UtcNow.AddDays(7);

        var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForDownloadAsync(
            uploadedFilename,
            preSignedUrlExpiresAt);

        return new Attachment(
            _idGenerator.CreateId(),
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
        if (!await _fileStorage.IsObjectExistsAsync(uploadedFilename, cancellationToken))
        {
            return ApiErrors.File.NotFound(uploadedFilename);
        }
        await _fileStorage.DeleteObjectAsync(uploadedFilename, cancellationToken);
        return true;
    }

    private string GenerateUploadFilename(string filename, long channelId)
    {
        return $"attachment/{channelId}/{Guid.NewGuid()}/{filename}";
    }
}

