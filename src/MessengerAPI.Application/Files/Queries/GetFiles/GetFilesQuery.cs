using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Files.Queries.GetFiles;

/// <summary>
/// Get user files
/// </summary>
/// <param name="Sub">User id</param>
public record GetFilesQuery(Guid Sub) : IRequest<ErrorOr<List<FileSchema>>>;
