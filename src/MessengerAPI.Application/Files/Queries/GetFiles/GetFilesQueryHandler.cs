using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Files.Queries.GetFiles;

public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, ErrorOr<List<FileSchema>>>
{
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public GetFilesQueryHandler(IFileRepository fileRepository, IMapper mapper)
    {
        _fileRepository = fileRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get user files
    /// </summary>
    /// <param name="request"><see cref="GetFilesQuery"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>A list of user files<see cref="FileSchema"/></returns>
    public async Task<ErrorOr<List<FileSchema>>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _fileRepository.GetUserFilesAsync(request.Sub, cancellationToken);
        return _mapper.Map<List<FileSchema>>(files);
    }
}
