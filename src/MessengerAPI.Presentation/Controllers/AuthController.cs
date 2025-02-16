using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Presentation.Schemas.Auth;
using MessengerAPI.Application.Auth.Commands.Login;
using MessengerAPI.Application.Auth.Commands.RefreshToken;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;
using MessengerAPI.Core;
using Newtonsoft.Json;

namespace MessengerAPI.Presentation.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up")]
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
    [ProducesResponseType(typeof(TokenPairResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignInAsync([FromForm] SignInRequestSchema schema, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(schema.login, schema.password, schema.captchaToken);
        var loginResult = await _mediator.Send(command, cancellationToken);

        return loginResult.Match(
            success =>
            {
                AddRefreshTokenToCookies(success);
                return Ok(success);
            },
            errors => Problem(errors));
    }

    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenPairResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync([FromForm] string refreshToken, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(refreshToken);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success =>
            {
                AddRefreshTokenToCookies(success);
                return Ok(success);
            },
            errors => Problem(errors));
    }

    [HttpGet("token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Token()
    {
        string? sessionInfo = Request.Cookies[MessengerConstants.Auth.SessionCookieName];
        if (sessionInfo == null)
        {
            return Problem(ApiErrors.Auth.NoSessionInfoFound);
        }
        return Ok(JsonConvert.DeserializeObject<SessionSchema>(sessionInfo));
    }

    private void AddRefreshTokenToCookies(TokenPairResponse tokenPair)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            // Secure = true,
        };
        Response.Cookies.Append(
            MessengerConstants.Auth.SessionCookieName,
            JsonConvert.SerializeObject(new SessionSchema(
                tokenPair.RefreshToken,
                tokenPair.AccessToken,
                tokenPair.TokenType,
                tokenPair.ExpiresIn,
                DateTimeOffset.UtcNow)),
            cookieOptions);
    }
}
