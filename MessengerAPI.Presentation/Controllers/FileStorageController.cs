using MediatR;
using MessengerAPI.Application.Files.Commands.UploadFile;
using MessengerAPI.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("files")]
public class FileStorageController : ApiController
{
    private readonly IMediator _mediator;

    public FileStorageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        Stream fileStream = file.OpenReadStream();
        var sub = User.GetUserId();

        var command = new UploadFileCommand(sub, fileStream, file.ContentType);

        var result = await _mediator.Send(command);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
