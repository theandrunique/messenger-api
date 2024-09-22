using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Files.Commands.UploadFile;

public record UploadFileCommand(
    UserId Sub,
    Stream FileStream,
    string ContentType,
    string FileName
) : IRequest<ErrorOr<FileSchema>>;
