using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Files.Queries.GetFiles;

/// <summary>
/// Get user files
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
public record GetFilesQuery(Guid Sub) : IRequest<ErrorOr<List<FileSchema>>>;
