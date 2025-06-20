using MediatR;
using Messenger.Application.Channels.Commands.DeleteCloudAttachment;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

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

        var command = new DeleteCloudAttachmentCommand(decodedUploadFilename);
        var result = await _mediator.Send(command, token);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }
}

