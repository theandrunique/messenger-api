using System.Security.Cryptography;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Entities;

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
        var sha265String = Convert.ToHexString(sha256Bytes).ToLower();

        var key = $"{sha265String}-{request.FileName}-{DateTime.UtcNow}";

        request.FileStream.Position = 0;
        
        var url = await _fileStorage.Put(request.FileStream, key, request.FileName, request.ContentType);

        var file = FileData.CreateNew(request.Sub, request.ContentType, request.FileName, url, request.FileStream.Length, sha256Bytes);

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
}
