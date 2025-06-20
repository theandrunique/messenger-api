using MediatR;
using Messenger.Application.Channels.Commands.AckMessage;
using Messenger.Application.Channels.Commands.AddChannelMember;
using Messenger.Application.Channels.Commands.AddOrEditMessage;
using Messenger.Application.Channels.Commands.CreateCloudAttachments;
using Messenger.Application.Channels.Commands.RemoveChannelMember;
using Messenger.Application.Channels.Commands.DeleteMessage;
using Messenger.Application.Channels.Commands.UpdateChannel;
using Messenger.Application.Channels.Common;
using Messenger.Application.Channels.Queries.GetAttachments;
using Messenger.Application.Channels.Queries.GetChannel;
using Messenger.Application.Channels.Queries.GetMessageAcks;
using Messenger.Application.Channels.Queries.GetMessages;
using Messenger.Contracts.Common;
using Messenger.Presentation.Schemas.Channels;
using Microsoft.AspNetCore.Mvc;
using Messenger.Application.Channels.Queries.GetMessage;
using Messenger.Application.Channels.Commands.ForwardMessages;

namespace Messenger.Presentation.Controllers;

[Route("channels")]
public class ChannelsController : ApiController
{
    private readonly IMediator _mediator;

    public ChannelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{channelId}")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChannelAsync(long channelId, CancellationToken cancellationToken)
    {
        var query = new GetChannelQuery(channelId);
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPatch("{channelId}")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateChannelAsync(
        long channelId,
        [FromForm] UpdateChannelRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new UpdateChannelCommand(channelId, schema.name, schema.image);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
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
            schema.referencedMessageId,
            channelId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("{channelId}/messages/forward")]
    public async Task<IActionResult> ForwardMessagesAsync(
        long channelId,
        ForwardMessagesSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new ForwardMessagesCommand(
            channelId,
            schema.messages,
            schema.targetChannelId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpDelete("{channelId}/messages/{messageId}")]
    public async Task<IActionResult> DeleteMessageAsync(
        long channelId,
        long messageId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteMessageCommand(channelId, messageId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: (_) => NoContent(), onError: Problem);
    }

    [HttpGet("{channelId}/messages/{messageId}")]
    [ProducesResponseType(typeof(List<MessageSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessagesAsync(
        long channelId,
        long messageId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMessageQuery(channelId, messageId);
        var result = await _mediator.Send(query, cancellationToken);
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

    [HttpGet("{channelId}/attachments")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChannelAttachmentsAsync(
        long channelId,
        long? before = null,
        int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var actualBefore = before ?? long.MaxValue;

        var query = new GetAttachmentsQuery(channelId, actualBefore, limit);
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
            schema.referencedMessageId,
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
        var command = new CreateCloudAttachmentsCommand(
            channelId,
            schema.files.ConvertAll(f => new UploadAttachmentDto
            {
                Id = f.id,
                Filename = f.filename,
                FileSize = f.fileSize,
            }));
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{channelId}/members/{userId}")]
    public async Task<IActionResult> AddMemberToChannelAsync(
        long channelId,
        long userId,
        CancellationToken cancellationToken)
    {
        var command = new AddChannelMemberCommand(channelId, userId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpDelete("{channelId}/members/{userId}")]
    public async Task<IActionResult> RemoveMemberFromChannelAsync(
        long channelId,
        long userId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveChannelMemberCommand(channelId, userId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPost("{channelId}/messages/{messageId}/acks")]
    public async Task<IActionResult> AckMessageAsync(
        long channelId,
        long messageId,
        CancellationToken cancellationToken)
    {
        var command = new AckMessageCommand(channelId, messageId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpGet("{channelId}/messages/{messageId}/acks")]
    public async Task<IActionResult> GetMessageAcksAsync(
        long channelId,
        long messageId,
        CancellationToken cancellationToken)
    {
        var query = new GetMessageAcksQuery(channelId, messageId);
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }
}
