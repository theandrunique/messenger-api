using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Application.Files.Queries.GetFiles;

public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, ErrorOr<List<FileData>>>
{
    private readonly IFileRepository _fileRepository;

    public GetFilesQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<ErrorOr<List<FileData>>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _fileRepository.GetUserFilesAsync(request.Sub);
        return files;
    }
}
