using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Messenger.Application.Common.Interfaces.S3;

namespace Messenger.Infrastructure.Common.Files;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly TransferUtility _transferUtility;

    public S3Service(IAmazonS3 client)
    {
        _s3Client = client;
        _transferUtility = new TransferUtility(_s3Client);
    }

    public Task PutObjectAsync(
        string key,
        string bucket,
        string contentType,
        Stream fileStream,
        string? fileName = null,
        CancellationToken cancellationToken = default)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = key,
            BucketName = bucket,
            ContentType = contentType,
            CalculateContentMD5Header = true,
        };
        if (fileName != null)
        {
            uploadRequest.Headers.ContentDisposition = "attachment; filename=\"" + fileName + "\"";
        }

        return _transferUtility.UploadAsync(uploadRequest, cancellationToken);
    }

    public string GenerateUploadUrl(string key,
        string bucket,
        DateTimeOffset expires,
        long size)
        => GeneratePreSignedUrl(key, expires, bucket, HttpVerb.PUT, size);

    public string GenerateDownloadUrl(
        string key,
        string bucket,
        DateTimeOffset expires)
        => GeneratePreSignedUrl(key, expires, bucket, HttpVerb.GET);

    private string GeneratePreSignedUrl(
        string key,
        DateTimeOffset expires,
        string bucket,
        HttpVerb httpVerb,
        long size = 0)
    {
        var getPreSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = key,
            Expires = expires.UtcDateTime,
            Verb = httpVerb,
        };
        if (httpVerb == HttpVerb.PUT)
        {
            getPreSignedUrlRequest.Headers.ContentLength = size;
        }

        return _s3Client.GetPreSignedURL(getPreSignedUrlRequest);
    }

    public async Task<GetObjectMetadataResponseDTO?> GetObjectMetadataAsync(
        string key,
        string bucket,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var getObjectMetadataRequest = new GetObjectMetadataRequest
            {
                BucketName = bucket,
                Key = key
            };
            var response = await _s3Client.GetObjectMetadataAsync(getObjectMetadataRequest, cancellationToken);

            return new GetObjectMetadataResponseDTO
            {
                ContentType = response.Headers.ContentType,
                ObjectSize = response.Headers.ContentLength,
            };
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch
        {
            throw;
        }
    }

    public Task DeleteObjectAsync(
        string key,
        string bucket,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = key,
        };

        return _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    public async Task<bool> IsObjectExistsAsync(
        string key,
        string bucket,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _s3Client.GetObjectMetadataAsync(bucket, key, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch
        {
            throw;
        }
    }
}
