using System.Text.RegularExpressions;
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
        var uploadFilename = GenerateUploadFilename(filename, channelId, _idGenerator.CreateId());

        var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForUploadAsync(
            uploadFilename,
            DateTimeOffset.UtcNow.AddDays(1),
            size
        );
        return new UploadUrlDto(uploadFilename, preSignedUrl);
    }

    public async Task<ErrorOr<Attachment>> ValidateAndCreateAttachmentsAsync(
        string uploadedFilename,
        string filename,
        CancellationToken cancellationToken)
    {
        var parsedFilename = ParseUploadedFilename(uploadedFilename);
        if (parsedFilename == null)
        {
            return ApiErrors.File.InvalidUploadFilename(uploadedFilename);
        }

        var objectMetadata = await _fileStorage.GetObjectMetadataAsync(uploadedFilename, cancellationToken);

        if (objectMetadata is null)
        {
            return ApiErrors.File.NotFound(uploadedFilename);
        }

        var preSignedUrlExpiresAt = DateTimeOffset.UtcNow.AddDays(7);

        var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForDownloadAsync(
            uploadedFilename,
            preSignedUrlExpiresAt);

        return new Attachment(
            parsedFilename.Value.AttachmentId,
            null,
            parsedFilename.Value.ChannelId,
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

    private string GenerateUploadFilename(string filename, long channelId, long attachmentId)
    {
        return $"attachments/{channelId}/{attachmentId}/{filename}";
    }

    public (long ChannelId, long AttachmentId, string Filename)? ParseUploadedFilename(string uploadedFilename)
    {
        var regex = new Regex(@"attachments\/(?<channelId>\d+)\/(?<attachmentId>\d+)\/(?<filename>.+)");
        var match = regex.Match(uploadedFilename);
        if (!match.Success)
        {
            return null;
        }

        var channelId = long.Parse(match.Groups["channelId"].Value);
        var attachmentId = long.Parse(match.Groups["attachmentId"].Value);
        var filename = match.Groups["filename"].Value;

        return (channelId, attachmentId, filename);
    }

}

