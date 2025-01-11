using MediatR;
using MessengerAPI.Application.Channels.Commands;
using MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;
using MessengerAPI.Application.Channels.Commands.PostAttachment;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Channels.Queries.GetChannels;
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

    /// <summary>
    /// Get user's channels
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="ChannelSchema"/></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChannelsAsync(CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new GetChannelsQuery(identity.UserId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Create channel
    /// </summary>
    /// <param name="schema"><see cref="CreateChannelRequestSchema"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="ChannelSchema"/></returns>
    [HttpPost]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateChannelAsync(CreateChannelRequestSchema schema, CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new CreateChannelCommand(
            identity.UserId,
            schema.members,
            schema.type,
            schema.title);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Create message
    /// </summary>
    /// <param name="schema"><see cref="CreateMessageRequestSchema"/></param>
    /// <param name="channelId">Id of channel</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="MessageSchema"/></returns>
    [HttpPost("{channelId}/messages")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateMessageAsync(
        CreateMessageRequestSchema schema,
        Guid channelId,
        CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new AddOrUpdateMessageCommand(
            null,
            identity.UserId,
            channelId,
            schema.text,
            schema.replyTo,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Get messages from channel
    /// </summary>
    /// <param name="channelId">Channel id</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <param name="offset">Offset</param>
    /// <param name="limit">Limit</param>
    /// <returns><see cref="MessageSchema"/></returns>
    [HttpGet("{channelId}/messages")]
    [ProducesResponseType(typeof(List<MessageSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessagesAsync(
        Guid channelId,
        CancellationToken cancellationToken,
        Guid? before = null,
        int limit = 50)
    {
        var identity = User.GetIdentity();

        var actualBefore = before ?? Guid.Empty;

        var query = new GetMessagesQuery(identity.UserId, channelId, actualBefore, limit);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Edit message
    /// </summary>
    /// <param name="schema"><see cref="CreateMessageRequestSchema"/></param>
    /// <param name="channelId">Channel id</param>
    /// <param name="messageId">Message id</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    [HttpPut("{channelId}/messages/{messageId}")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessageAsync(
        CreateMessageRequestSchema schema,
        Guid channelId,
        Guid messageId,
        CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new AddOrUpdateMessageCommand(
            messageId,
            identity.UserId,
            channelId,
            schema.text,
            schema.replyTo,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPost("{channelId}/attachments")]
    public async Task<IActionResult> PostAttachmentAsync(
        Guid channelId,
        CreateChannelAttachmentSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new PostAttachmentCommand(
            channelId,
            schema.files.ConvertAll(f => new FileData
            {
                Filename = f.filename,
                Size = f.size,
            }));

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors));
    }

}
