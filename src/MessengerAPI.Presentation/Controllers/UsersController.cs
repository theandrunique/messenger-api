using Amazon.Runtime.CredentialManagement;
using MediatR;
using MessengerAPI.Application.Channels.Commands;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessagePrivateChannel;
using MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;
using MessengerAPI.Application.Channels.Queries.GetChannels;
using MessengerAPI.Application.Channels.Queries.GetMessagesPrivateChannel;
using MessengerAPI.Application.Users.Commands;
using MessengerAPI.Application.Users.Commands.RemoveAvatar;
using MessengerAPI.Application.Users.Commands.RequestVerifyEmailCode;
using MessengerAPI.Application.Users.Commands.UpdateAvatar;
using MessengerAPI.Application.Users.Commands.VerifyEmail;
using MessengerAPI.Application.Users.Queries.GetMeQuery;
using MessengerAPI.Application.Users.Queries.GetUserById;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Infrastructure.Auth;
using MessengerAPI.Presentation.Schemas.Auth;
using MessengerAPI.Presentation.Schemas.Channels;
using MessengerAPI.Presentation.Schemas.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace MessengerAPI.Presentation.Controllers;

[Route("users")]
public class UsersController : ApiController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserPublicSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAsync(long userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("@me")]
    [ProducesResponseType(typeof(UserPrivateSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMeAsync(CancellationToken cancellationToken)
    {
        var query = new GetMeQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPut("@me/avatar")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAvatarAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var command = new UpdateAvatarCommand(file);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpDelete("@me/avatar")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAvatarAsync(CancellationToken cancellationToken)
    {
        var command = new RemoveAvatarCommand();
        var _ = await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("@me/email/request-verify-code")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RequestEmailCodeAsync(CancellationToken cancellationToken)
    {
        var command = new RequestVerifyEmailCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPost("@me/email/verify")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> VerifyEmailCodeAsync(
        VerifyEmailRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new VerifyEmailCommand(schema.code);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPost("@me/mfa/totp/enable")]
    public async Task<IActionResult> EnableMfaTotpAsync(
        MfaTotpEnableRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new MfaTotpEnableCommand(schema.password, schema.emailCode);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("@me/channels")]
    [ProducesResponseType(typeof(List<ChannelSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyChannelsAsync(CancellationToken cancellationToken)
    {
        var query = new GetChannelsQuery();

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("@me/private-channel/{userId}")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrivateChannelAsync(long userId, CancellationToken cancellationToken)
    {
        var command = new GetOrCreatePrivateChannelCommand(userId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("@me/private-channel/me")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSavedMessagesChannelAsync(CancellationToken cancellationToken)
    {
        var command = new GetOrCreatePrivateChannelCommand(User.GetUserId());

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("@me/channels")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateChannelAsync(CreateChannelRequestSchema schema, CancellationToken cancellationToken)
    {
        var query = new CreateChannelCommand(schema.members, schema.title);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("{userId}/messages")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendMessageToPrivateChannelAsync(
        long userId,
        CreateMessageRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new AddOrEditMessagePrivateChannelCommand(
            userId,
            null,
            userId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPut("{userId}/messages/{messageId}")]
    [ProducesResponseType(typeof(MessageSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditMessageInPrivateChannelAsync(
        long userId,
        long messageId,
        CreateMessageRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var command = new AddOrEditMessagePrivateChannelCommand(
            userId,
            messageId,
            userId,
            schema.content,
            schema.attachments);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }


    [HttpGet("{userId}/messages")]
    [ProducesResponseType(typeof(List<MessageSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessagesPrivateChannelAsync(
        long userId,
        long? before = null,
        int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var actualBefore = before ?? long.MaxValue;

        var query = new GetMessagesPrivateChannelQuery(userId, actualBefore, limit);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }
}
