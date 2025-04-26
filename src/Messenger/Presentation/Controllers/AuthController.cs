using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Messenger.Application.Auth.Commands.Register;
using Messenger.Presentation.Schemas.Auth;
using Messenger.Application.Auth.Commands.Login;
using Messenger.Application.Auth.Commands.RefreshToken;
using Messenger.Application.Auth.Common;
using Messenger.Contracts.Common;
using Messenger.Errors;
using Messenger.Core;
using Newtonsoft.Json;
using Messenger.Application.Auth.Commands.Logout;

namespace Messenger.Presentation.Controllers;

[Route("auth")]
public class AuthController : ApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserPrivateSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignUpAsync([FromForm] SignUpRequestSchema schema, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            schema.username,
            schema.email,
            schema.globalName,
            schema.password);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenPairResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignInAsync([FromForm] SignInRequestSchema schema, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(schema.login, schema.password, schema.totp);
        var loginResult = await _mediator.Send(command, cancellationToken);

        return loginResult.Match(
            success =>
            {
                AddSessionCookies(success);
                return Ok(success);
            },
            errors => Problem(errors));
    }

    [HttpPost("sign-out")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        var command = new LogoutCommand();
        await _mediator.Send(command, cancellationToken);
        RemoveSessionCookie();
        return NoContent();
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenPairResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync([FromForm] string refreshToken, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(refreshToken);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success =>
            {
                AddSessionCookies(success);
                return Ok(success);
            },
            errors => Problem(errors));
    }

    [HttpGet("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenPairResponse), StatusCodes.Status200OK)]
    public IActionResult Token()
    {
        string? sessionInfo = Request.Cookies[MessengerConstants.Auth.SessionCookieName];
        if (sessionInfo == null)
        {
            return Problem(ApiErrors.Auth.NoSessionInfoFound);
        }
        return Ok(JsonConvert.DeserializeObject<TokenPairResponse>(sessionInfo));
    }

    private void AddSessionCookies(TokenPairResponse tokenPair)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = tokenPair.IssuedAt.AddDays(7),
            HttpOnly = true,
            // Secure = true,
        };
        Response.Cookies.Append(
            MessengerConstants.Auth.SessionCookieName,
            JsonConvert.SerializeObject(tokenPair),
            cookieOptions);
    }

    private void RemoveSessionCookie()
    {
        Response.Cookies.Delete(MessengerConstants.Auth.SessionCookieName);
    }
}
