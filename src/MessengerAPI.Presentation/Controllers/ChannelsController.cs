using MediatR;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessage;
using MessengerAPI.Application.Channels.Commands.CreateAttachment;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Channels.Queries.GetMessages;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Presentation.Common;
using MessengerAPI.Presentation.Schemas.Channels;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("channels")]
public class ChannelsController : ApiController
{
    private readonly IMediator _mediator;

    public ChannelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{channelId}/messages")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateMessageAsync(
        CreateMessageRequestSchema schema,
        long channelId,
        CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new AddOrEditMessageCommand(
            null,
            identity.UserId,
            channelId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpGet("{channelId}/messages")]
    [ProducesResponseType(typeof(List<MessageSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessagesAsync(
        long channelId,
        CancellationToken cancellationToken,
        long? before = null,
        int limit = 50)
    {
        var identity = User.GetIdentity();

        var actualBefore = before ?? long.MaxValue;

        var query = new GetMessagesQuery(identity.UserId, channelId, actualBefore, limit);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPut("{channelId}/messages/{messageId}")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessageAsync(
        CreateMessageRequestSchema schema,
        long channelId,
        long messageId,
        CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new AddOrEditMessageCommand(
            messageId,
            identity.UserId,
            channelId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPost("{channelId}/attachments")]
    public async Task<IActionResult> PostAttachmentAsync(
        long channelId,
        CreateChannelAttachmentSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new CreateAttachmentCommand(
            channelId,
            schema.files.ConvertAll(f => new UploadAttachmentDto
            {
                Id = f.id,
                Filename = f.filename,
                FileSize = f.fileSize,
            }));

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors));
    }

}
