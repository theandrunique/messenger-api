using System.Security.Cryptography;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Common.Interfaces.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Channels.Common;

public class ChannelImageService
{
    private readonly IS3Service _s3Service;
    private readonly ChannelImageServiceOptions _options;
    private readonly IImageProcessor _imageProcessor;

    public ChannelImageService(
        IS3Service s3Service,
        IOptions<ChannelImageServiceOptions> options,
        IImageProcessor imageProcessor)
    {
        _s3Service = s3Service;
        _options = options.Value;
        _imageProcessor = imageProcessor;
    }

    public async Task<string> UploadImage(IFormFile file, long channelId, CancellationToken ct)
    {
        using var processedImage = await _imageProcessor.ProcessImageAsWebp(file, 512, 512);
        var mimeType = file.ContentType.ToLower();
        var result = GenerateUploadFilename(file, channelId);

        await _s3Service.PutObjectAsync(
            key: result.uploadFilename,
            bucket: _options.BucketName,
            contentType: mimeType,
            fileStream: processedImage,
            cancellationToken: ct);

        return result.hash;
    }

    private (string hash, string uploadFilename) GenerateUploadFilename(IFormFile file, long channelId)
    {
        var sha = SHA1.Create();
        var hash = sha.ComputeHash(file.OpenReadStream());
        var hex = Convert.ToHexString(hash).ToLower();

        return (hex, $"/channels/{channelId}/images/{hex}");
    }
}
