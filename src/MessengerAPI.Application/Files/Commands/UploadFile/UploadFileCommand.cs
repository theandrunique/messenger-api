using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Files.Commands.UploadFile;

/// <summary>
/// Uploading file to S3 and saving it in DB
/// </summary>
/// <param name="Sub">User id</param>
/// <param name="FileStream">Stream of file</param>
/// <param name="ContentType">Content type</param>
/// <param name="FileName">name of file</param>
public record UploadFileCommand(
    Guid Sub,
    Stream FileStream,
    string ContentType,
    string FileName
) : IRequest<ErrorOr<FileSchema>>;
