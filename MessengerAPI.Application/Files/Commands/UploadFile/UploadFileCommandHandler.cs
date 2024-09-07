using System.Security.Cryptography;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Application.Files.Commands.UploadFile;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, ErrorOr<FileData>>
{
    IFileStorage _fileStorage;
    IFileRepository _fileRepository;

    public UploadFileCommandHandler(IFileStorage fileStorage, IFileRepository fileRepository)
    {
        _fileStorage = fileStorage;
        _fileRepository = fileRepository;
    }

    public async Task<ErrorOr<FileData>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var sha256Bytes = ComputeSha256Hash(request.FileStream);
        var sha265String = Convert.ToHexString(sha256Bytes);

        var fileType = DetermineFileType(request.ContentType);

        request.FileStream.Position = 0;
        
        var url = await _fileStorage.Put(request.FileStream, sha265String);

        var file = FileData.CreateNew(fileType, url, request.FileStream.Length, sha256Bytes);

        await _fileRepository.AddFileAsync(file);
        await _fileRepository.Commit();

        return file;
    }

    private byte[] ComputeSha256Hash(Stream stream)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(stream);
        }
    }

    private FileType DetermineFileType(string contentType)
    {
        return contentType switch
        {
            "image/jpeg" => FileType.Image,
            "image/png" => FileType.Image,
            "video/mp4" => FileType.Video,
            "application/pdf" => FileType.Document,
            "audio/mpeg" => FileType.Audio,
            _ => FileType.Other,
        };
    }
}
