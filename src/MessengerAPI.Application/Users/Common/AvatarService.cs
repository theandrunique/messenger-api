using System.Security.Cryptography;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Application.Users.Common;

public class AvatarService
{
    private readonly IS3Service _s3Service;
    private readonly IClientInfoProvider _clientInfo;
    private readonly AvatarServiceOptions _options;
    private readonly IImageProcessor _imageProcessor;

    public AvatarService(
        IS3Service s3Service,
        IClientInfoProvider clientInfo,
        IOptions<AvatarServiceOptions> options,
        IImageProcessor imageProcessor)
    {
        _s3Service = s3Service;
        _clientInfo = clientInfo;
        _options = options.Value;
        _imageProcessor = imageProcessor;
    }

    public async Task<string> UploadAvatar(IFormFile file, CancellationToken ct)
    {
        using var processedImage = await _imageProcessor.ProcessImage(file);
        var mimeType = file.ContentType.ToLower();
        var result = GenerateUploadFilename(file);

        Console.WriteLine($"Uploaded filename: {result.uploadFilename}");

        await _s3Service.PutObjectAsync(
            key: result.uploadFilename,
            bucket: _options.BucketName,
            fileName: file.FileName,
            contentType: mimeType,
            fileStream: processedImage,
            cancellationToken: ct);

        return result.hash;
    }

    private (string hash, string uploadFilename) GenerateUploadFilename(IFormFile file)
    {
        var sha = SHA1.Create();
        var hash = sha.ComputeHash(file.OpenReadStream());
        var hex = Convert.ToHexString(hash).ToLower();

        return (hex, $"avatars/{_clientInfo.UserId}/{hex}");
    }
}
