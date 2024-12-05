using MediatR;
using MessengerAPI.Application.Users.Commands.VerifyEmail;
using MessengerAPI.Application.Users.Queries.GetMeQuery;
using MessengerAPI.Application.Users.Queries.RequestVerifyEmail;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Presentation.Common;
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

    /// <summary>
    /// Get current user private data
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="UserPrivateSchema"/></returns>
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

    [HttpGet("verification-code")]
    public async Task<IActionResult> RequestEmailVerificationCodeAsync(CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var query = new RequestVerifyEmailCommand(identity.UserId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmailAsync([FromQuery] string code, CancellationToken cancellationToken)
    {
        var identity = User.GetIdentity();

        var command = new VerifyEmailCommand(identity.UserId, code);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors));
    }
}
