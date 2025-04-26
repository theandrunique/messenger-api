using MediatR;
using Messenger.Application.Channels.Commands;
using Messenger.Application.Channels.Commands.GetDMChannel;
using Messenger.Application.Channels.Queries.GetChannels;
using Messenger.Application.Users.Commands;
using Messenger.Application.Users.Commands.RemoveAvatar;
using Messenger.Application.Users.Commands.RequestVerifyEmailCode;
using Messenger.Application.Users.Commands.UpdateAvatar;
using Messenger.Application.Users.Commands.VerifyEmail;
using Messenger.Application.Users.Queries.GetMeQuery;
using Messenger.Application.Users.Queries.GetSessions;
using Messenger.Application.Users.Queries.GetUserById;
using Messenger.Application.Users.Queries.SearchUsers;
using Messenger.Contracts.Common;
using Messenger.Infrastructure.Auth;
using Messenger.Presentation.Schemas.Auth;
using Messenger.Presentation.Schemas.Channels;
using Messenger.Presentation.Schemas.Users;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

[Route("users")]
public class UsersController : ApiController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<UserPublicSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchUsersAsync(string query, CancellationToken cancellationToken)
    {
        var searchQuery = new SearchUsersQuery(query);
        var result = await _mediator.Send(searchQuery, cancellationToken);
        return Ok(result);
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

    [HttpGet("@me/sessions")]
    [ProducesResponseType(typeof(List<SessionSchema>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMySessionsAsync(CancellationToken cancellationToken)
    {
        var query = new GetSessionsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
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

    [HttpGet("@me/dms/{userId}")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrivateChannelAsync(long userId, CancellationToken cancellationToken)
    {
        var command = new GetDMChannelCommand(userId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("@me/dms/me")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSavedMessagesChannelAsync(CancellationToken cancellationToken)
    {
        var command = new GetDMChannelCommand(User.GetUserId());
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("@me/channels")]
    [ProducesResponseType(typeof(ChannelSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateChannelAsync(
        CreateChannelRequestSchema schema,
        CancellationToken cancellationToken)
    {
        var query = new CreateChannelCommand(schema.members, schema.name);
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }
}
