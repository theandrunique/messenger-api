using ErrorOr;
using MediatR;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Files.Commands.UploadFile;

public record UploadFileCommand(
    UserId Sub,
    Stream FileStream,
    string ContentType,
    string FileName
) : IRequest<ErrorOr<FileData>>;
