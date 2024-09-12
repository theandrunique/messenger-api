using System.Security.Cryptography;
using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Application.Files.Commands.UploadFile;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, ErrorOr<FileSchema>>
{
    private readonly IFileStorage _fileStorage;
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public UploadFileCommandHandler(IFileStorage fileStorage, IFileRepository fileRepository, IMapper mapper)
    {
        _fileStorage = fileStorage;
        _fileRepository = fileRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<FileSchema>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var sha256Bytes = ComputeSha256Hash(request.FileStream);
        var sha265String = Convert.ToHexString(sha256Bytes).ToLower();

        var key = $"{sha265String}-{request.FileName}-{DateTime.UtcNow}";

        request.FileStream.Position = 0;
        
        var url = await _fileStorage.Put(request.FileStream, key, request.FileName, request.ContentType);

        var file = FileData.CreateNew(request.Sub, request.ContentType, request.FileName, url, request.FileStream.Length, sha256Bytes);

        await _fileRepository.AddFileAsync(file);
        await _fileRepository.Commit();

        return _mapper.Map<FileSchema>(file);
    }

    private byte[] ComputeSha256Hash(Stream stream)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(stream);
        }
    }
}
