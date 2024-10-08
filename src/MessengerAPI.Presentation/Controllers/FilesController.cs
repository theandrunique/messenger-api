using MediatR;
using MessengerAPI.Application.Files.Commands.UploadFile;
using MessengerAPI.Application.Files.Queries.GetFiles;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Infrastructure.Common.Files;
using MessengerAPI.Presentation.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Presentation.Controllers;

[Route("files")]
public class FilesController : ApiController
{
    private readonly IMediator _mediator;
    private readonly StorageOptions _settings;

    public FilesController(IMediator mediator, IOptions<StorageOptions> settings)
    {
        _mediator = mediator;
        _settings = settings.Value;
    }

    /// <summary>
    /// Upload file
    /// </summary>
    /// <param name="file"><see cref="IFormFile"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="FileSchema"/></returns>
    [HttpPost]
    [ProducesResponseType(typeof(FileSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length > _settings.MaxFileSize)
        {
            return Problem(Errors.File.TooBig(_settings.MaxFileSizeInMB));
        }

        using Stream fileStream = file.OpenReadStream();

        var identity = User.GetIdentity();

        var command = new UploadFileCommand(identity.UserId, fileStream, file.ContentType, file.FileName);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Get user's files
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>List of <see cref="FileSchema"/></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<FileSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFilesAsync(CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new GetFilesQuery(identity.UserId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
