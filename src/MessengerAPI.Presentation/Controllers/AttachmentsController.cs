using MediatR;
using MessengerAPI.Application.Channels.Commands.DeleteAttachment;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("attachments")]
public class AttachmentsController : ApiController
{
    private readonly IMediator _mediator;

    public AttachmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{uploadFilename}")]
    public async Task<IActionResult> DeleteAttachmentAsync(string uploadFilename, CancellationToken token)
    {
        var decodedUploadFilename = Uri.UnescapeDataString(uploadFilename);

        var command = new DeleteAttachmentCommand(decodedUploadFilename);
        var result = await _mediator.Send(command, token);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }
}

