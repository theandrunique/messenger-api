using MediatR;
using MessengerAPI.Application.Channels.Commands;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessagePrivateChannel;
using MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;
using MessengerAPI.Application.Channels.Queries.GetChannels;
using MessengerAPI.Application.Channels.Queries.GetMessagesPrivateChannel;
using MessengerAPI.Application.Users.Queries.GetMeQuery;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Presentation.Common;
using MessengerAPI.Presentation.Schemas.Channels;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("users")]
public class UsersController : ApiController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me/channels")]
    [ProducesResponseType(typeof(List<ChannelSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyChannelsAsync(CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new GetChannelsQuery(identity.UserId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserPrivateSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMeAsync(CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new GetMeQuery(identity.UserId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpGet("me/private-channel/{userId}")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrivateChannelAsync(long userId, CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new GetOrCreatePrivateChannelCommand(identity.UserId, userId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpGet("me/private-channel/me")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSavedMessagesChannelAsync(CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new GetOrCreatePrivateChannelCommand(identity.UserId, identity.UserId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPost("me/channels")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateChannelAsync(CreateChannelRequestSchema schema, CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new CreateChannelCommand(
            identity.UserId,
            schema.members,
            schema.title);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPost("{userId}/messages")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendMessageToPrivateChannelAsync(
        long userId,
        CreateMessageRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new AddOrEditMessagePrivateChannelCommand(
            userId,
            null,
            identity.UserId,
            userId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPut("{userId}/messages/{messageId}")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessageInPrivateChannelAsync(
        long userId,
        long messageId,
        CreateMessageRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new AddOrEditMessagePrivateChannelCommand(
            userId,
            messageId,
            identity.UserId,
            userId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }


    [HttpGet("{userId}/messages")]
    [ProducesResponseType(typeof(List<MessageSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessagesPrivateChannelAsync(
        long userId,
        CancellationToken cancellationToken,
        long? before = null,
        int limit = 50)
    {
        var identity = User.GetIdentity();

        var actualBefore = before ?? long.MaxValue;

        var query = new GetMessagesPrivateChannelQuery(identity.UserId, userId, actualBefore, limit);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
