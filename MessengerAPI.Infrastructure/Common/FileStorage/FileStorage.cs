using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using MessengerAPI.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Infrastructure.Common.FileStorage;

public class FileStorage : IFileStorage
{
    private readonly FileStorageSettings _settings;
    private readonly IAmazonS3 _s3Client;

    public FileStorage(IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;

        var config = new AmazonS3Config()
        {
            ServiceURL = _settings.EndpointUrl,
        };
        AWSCredentials credentials = new BasicAWSCredentials(_settings.AccessKey, _settings.SecretKey);

        _s3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<string> Put(Stream fileStream, string key, string fileName, string contentType)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = key,
            BucketName = _settings.BucketName,
            PartSize = 6291456, // 6 MB
            ContentType = contentType,
            CalculateContentMD5Header = true,
        };
        uploadRequest.Headers.ContentDisposition = "attachment; filename=\"" + fileName + "\"";

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"{_settings.BucketUrl}/{key}";
    }
}
