using MediatR;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessage;
using MessengerAPI.Application.Channels.Commands.CreateAttachment;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Channels.Queries.GetMessages;
using MessengerAPI.Contracts.Common;
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
        var command = new AddOrEditMessageCommand(
            null,
            channelId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("{channelId}/messages")]
    [ProducesResponseType(typeof(List<MessageSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessagesAsync(
        long channelId,
        long? before = null,
        int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var actualBefore = before ?? long.MaxValue;

        var query = new GetMessagesQuery(channelId, actualBefore, limit);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPut("{channelId}/messages/{messageId}")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessageAsync(
        CreateMessageRequestSchema schema,
        long channelId,
        long messageId,
        CancellationToken cancellationToken)
    {
        var command = new AddOrEditMessageCommand(
            messageId,
            channelId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("{channelId}/attachments")]
    public async Task<IActionResult> PostAttachmentAsync(
        long channelId,
        CreateChannelAttachmentRequestSchema schema,
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

        return result.Match(onValue: Ok, onError: Problem);
    }
}
