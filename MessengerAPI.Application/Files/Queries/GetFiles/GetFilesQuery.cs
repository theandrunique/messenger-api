using ErrorOr;
using MediatR;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Files.Queries.GetFiles;

public record GetFilesQuery(UserId Sub) : IRequest<ErrorOr<List<FileData>>>;
