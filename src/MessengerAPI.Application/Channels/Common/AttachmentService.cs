using System.Text.RegularExpressions;
using MessengerAPI.Application.Common.Interfaces.S3;
using MessengerAPI.Core;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Errors;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Application.Channels.Common;

public class AttachmentService
{
    private readonly IS3Service _s3Service;
    private readonly IIdGenerator _idGenerator;
    private readonly AttachmentsOptions _options;

    public AttachmentService(IS3Service s3Service, IIdGenerator idGenerator, IOptions<AttachmentsOptions> options)
    {
        _s3Service = s3Service;
        _idGenerator = idGenerator;
        _options = options.Value;
    }

    public UploadUrlDto GenerateUploadUrl(long size, long channelId, string filename)
    {
        var uploadFilename = GenerateUploadFilename(filename, channelId, _idGenerator.CreateId());

        var preSignedUrl = _s3Service.GenerateUploadUrl(
            key: uploadFilename,
            bucket: _options.Bucket,
            expires: DateTimeOffset.UtcNow.Add(_options.UploadUrlExpiration),
            size: size);

        return new UploadUrlDto(uploadFilename, preSignedUrl);
    }

    public async Task<ErrorOr<Attachment>> ValidateAndCreateAttachmentsAsync(
        string uploadedFilename,
        string filename,
        CancellationToken cancellationToken = default)
    {
        var parsedFilename = ParseUploadedFilename(uploadedFilename);
        if (parsedFilename == null)
        {
            return ApiErrors.Attachment.InvalidUploadFilename(uploadedFilename);
        }

        var objectMetadata = await _s3Service.GetObjectMetadataAsync(
            uploadedFilename,
            _options.Bucket,
            cancellationToken);

        if (objectMetadata is null)
        {
            return ApiErrors.Attachment.NotFoundInObjectStorage(uploadedFilename);
        }

        var expires = DateTimeOffset.UtcNow.Add(_options.DownloadUrlExpiration);

        var preSignedUrl = _s3Service.GenerateDownloadUrl(
            key: uploadedFilename,
            bucket: _options.Bucket,
            expires: expires);

        return new Attachment(
            parsedFilename.Value.AttachmentId,
            null,
            parsedFilename.Value.ChannelId,
            filename,
            uploadedFilename,
            objectMetadata.ContentType,
            objectMetadata.ObjectSize,
            preSignedUrl,
            expires);
    }

    public Task DeleteAttachmentAsync(
        string uploadedFilename,
        CancellationToken cancellationToken = default)
        => _s3Service.DeleteObjectAsync(uploadedFilename, _options.Bucket, cancellationToken);

    public Task<bool> IsAttachmentsExistsAsync(string uploadedFilename, CancellationToken cancellationToken = default)
        => _s3Service.IsObjectExistsAsync(uploadedFilename, _options.Bucket, cancellationToken);

    private string GenerateUploadFilename(string filename, long channelId, long attachmentId)
    {
        return $"attachments/{channelId}/{attachmentId}/{filename}";
    }

    private (long ChannelId, long AttachmentId, string Filename)? ParseUploadedFilename(string uploadedFilename)
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

