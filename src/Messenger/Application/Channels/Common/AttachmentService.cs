using System.Text.RegularExpressions;
using Messenger.Application.Common.Interfaces.S3;
using Messenger.Core;
using Messenger.Errors;
using Microsoft.Extensions.Options;
using Messenger.Options;
using Messenger.Domain.Messages;
using Messenger.Domain.Data.Messages;

namespace Messenger.Application.Channels.Common;

public class AttachmentService
{
    private readonly IS3Service _s3Service;
    private readonly IIdGenerator _idGenerator;
    private readonly AttachmentServiceOptions _options;
    private readonly IAttachmentRepository _attachmentRepository;
    private static readonly Regex _uploadFilenameRegex =
        new Regex(@"^attachments/(?<channelId>\d+)/(?<attachmentId>\d+)/(?<filename>.+)$", RegexOptions.Compiled);

    public AttachmentService(
        IS3Service s3Service,
        IIdGenerator idGenerator,
        IAttachmentRepository attachmentRepository,
        IOptions<ApplicationOptions> options)
    {
        _s3Service = s3Service;
        _idGenerator = idGenerator;
        _attachmentRepository = attachmentRepository;
        _options = options.Value.AttachmentServiceOptions;
    }

    public Task UpdateUrlsAsync(long channelId, IEnumerable<Attachment> attachments)
    {
        List<Attachment> updated = new();

        foreach (var item in attachments)
        {
            if (item.IsNeedUpdateUrl())
            {
                var expires = DateTimeOffset.UtcNow.Add(_options.DownloadUrlExpiration);
                var uploadedFilename = GenerateUploadFilename(item.Filename, item.ChannelId, item.Id);
                var preSignedUrl = GenerateDownloadUrl(uploadedFilename, expires);

                item.UpdatePreSignedUrl(preSignedUrl, expires);
            }
        }
        return _attachmentRepository.UpdatePreSignedUrlsAsync(channelId, updated);
    }

    public UploadUrlDto GenerateUploadUrl(long size, long channelId, string filename)
    {
        var uploadFilename = GenerateUploadFilename(filename, channelId, _idGenerator.CreateId());

        var preSignedUrl = _s3Service.GenerateUploadUrl(
            key: uploadFilename,
            bucket: _options.BucketName,
            expires: DateTimeOffset.UtcNow.Add(_options.UploadUrlExpiration),
            size: size);

        return new UploadUrlDto(uploadFilename, preSignedUrl);
    }

    public async Task<ErrorOr<Attachment>> ValidateAndCreateAttachmentAsync(
        string uploadedFilename,
        string filename,
        CancellationToken cancellationToken = default)
    {
        var parsedFilename = ParseUploadedFilename(uploadedFilename);
        if (parsedFilename == null)
        {
            return Error.Attachment.InvalidUploadFilename(uploadedFilename);
        }

        var objectMetadata = await _s3Service.GetObjectMetadataAsync(
            uploadedFilename,
            _options.BucketName,
            cancellationToken);

        if (objectMetadata is null)
        {
            return Error.Attachment.NotFoundInObjectStorage(uploadedFilename);
        }

        var expires = DateTimeOffset.UtcNow.Add(_options.DownloadUrlExpiration);
        var preSignedUrl = GenerateDownloadUrl(uploadedFilename, expires);

        return new Attachment(
            id: parsedFilename.Value.AttachmentId,
            channelId: parsedFilename.Value.ChannelId,
            filename: filename,
            contentType: objectMetadata.ContentType,
            size: objectMetadata.ObjectSize,
            preSignedUrl: preSignedUrl,
            preSignedUrlExpiresTimestamp: expires);
    }

    public Task DeleteObjectAsync(
        string uploadedFilename,
        CancellationToken cancellationToken = default)
        => _s3Service.DeleteObjectAsync(uploadedFilename, _options.BucketName, cancellationToken);

    private string GenerateDownloadUrl(string uploadedFilename, DateTimeOffset expires)
    {
        return _s3Service.GenerateDownloadUrl(
            key: uploadedFilename,
            bucket: _options.BucketName,
            expires: expires);
    }

    public Task<bool> IsObjectExistsAsync(string uploadedFilename, CancellationToken cancellationToken = default)
        => _s3Service.IsObjectExistsAsync(uploadedFilename, _options.BucketName, cancellationToken);

    public async Task<Attachment?> FindAttachmentByUploadFilename(string uploadFilename)
    {
        var parsedUploadFilename = ParseUploadedFilename(uploadFilename);
        if (parsedUploadFilename == null)
        {
            throw new ArgumentException($"Invalid uploadFilename: {uploadFilename}");
        }

        return await _attachmentRepository.GetAttachmentOrNullAsync(
            parsedUploadFilename.Value.ChannelId,
            parsedUploadFilename.Value.AttachmentId);
    }

    private string GenerateUploadFilename(string filename, long channelId, long attachmentId)
    {
        return $"attachments/{channelId}/{attachmentId}/{filename}";
    }

    private (long ChannelId, long AttachmentId, string Filename)? ParseUploadedFilename(string uploadedFilename)
    {
        var match = _uploadFilenameRegex.Match(uploadedFilename);
        if (!match.Success)
        {
            return null;
        }

        if (!long.TryParse(match.Groups["channelId"].Value, out var channelId) ||
            !long.TryParse(match.Groups["attachmentId"].Value, out var attachmentId))
        {
            return null;
        }

        var filename = match.Groups["filename"].Value;
        return (channelId, attachmentId, filename);
    }
}
