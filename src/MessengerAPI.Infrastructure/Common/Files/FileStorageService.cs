using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MessengerAPI.Application.Common.Interfaces.Files;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Infrastructure.Common.Files;

public class FileStorageService : IFileStorageService
{
    private readonly StorageOptions _settings;
    private readonly IAmazonS3 _s3Client;

    public FileStorageService(IOptions<StorageOptions> settings)
    {
        _settings = settings.Value;

        var config = new AmazonS3Config()
        {
            ServiceURL = _settings.EndpointUrl,
        };
        AWSCredentials credentials = new BasicAWSCredentials(_settings.AccessKey, _settings.SecretKey);

        _s3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<string> PutObjectAsync(Stream fileStream, string key, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = key,
            BucketName = _settings.BucketName,
            PartSize = _settings.UploadPartSize,
            ContentType = contentType,
            CalculateContentMD5Header = true,
        };
        uploadRequest.Headers.ContentDisposition = "attachment; filename=\"" + fileName + "\"";

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

        return $"{_settings.BucketUrl}/{key}";
    }
    public Task<string> GeneratePreSignedUrlForUploadAsync(string key, DateTime expires, long size)
    {
        var getPreSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            Expires = expires,
            Verb = HttpVerb.PUT,
        };
        getPreSignedUrlRequest.Headers.ContentLength = size;

        return _s3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);
    }

    public Task<string> GeneratePreSignedUrlForDownloadAsync(string key, DateTime expires)
    {
        var response = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            Expires = expires,
            Verb = HttpVerb.GET
        };
        return _s3Client.GetPreSignedURLAsync(response);
    }

    public async Task<GetObjectMetadataResponseDTO> GetObjectMetadataAsync(string key, CancellationToken cancellationToken)
    {
        var getObjectMetadataRequest = new GetObjectMetadataRequest
        {
            BucketName = _settings.BucketName,
            Key = key
        };
        var response = await _s3Client.GetObjectMetadataAsync(getObjectMetadataRequest, cancellationToken);

        return new GetObjectMetadataResponseDTO
        {
            ContentType = response.Headers.ContentType,
            ObjectSize = response.Headers.ContentLength,
        };
    }
}
