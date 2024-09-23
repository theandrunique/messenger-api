using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Files.Queries.GetFiles;

/// <summary>
/// Get user files
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
public record GetFilesQuery(UserId Sub) : IRequest<ErrorOr<List<FileSchema>>>;
