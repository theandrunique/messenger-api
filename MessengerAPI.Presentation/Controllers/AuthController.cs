using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Presentation.Schemas.Auth;
using MessengerAPI.Application.Auth.Commands.Login;
using MessengerAPI.Presentation.Common;
using MessengerAPI.Application.Auth.Commands.RefreshToken;
using AutoMapper;

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
    public async Task<IActionResult> SignUp([FromForm] SignUpRequestSchema schema)
    {
        var command = new RegisterCommand(schema.Username, schema.GlobalName, schema.Password);

        var registerResult = await _mediator.Send(command);

        return registerResult.Match(
            success => Ok(success),
            errors => Problem(errors));
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromForm] SignInRequestSchema schema)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

        var command = new LoginCommand(schema.Login, schema.Password, userAgent, ipAddress);

        var loginResult = await _mediator.Send(command);

        return loginResult.Match(
            success =>
            {
                AddRefreshTokenToCookies(success.RefreshToken);
                return Ok(success);
            },
            errors => Problem(errors));
    }

    [HttpPost("token")]
    public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequestSchema schema)
    {
        var command = new RefreshTokenCommand(schema.RefreshToken);
        var result = await _mediator.Send(command);

        return result.Match(
            success =>
            {
                AddRefreshTokenToCookies(success.RefreshToken);
                return Ok(success);
            },
            errors => Problem(errors));
    }

    [HttpGet("token")]
    public IActionResult Token()
    {
        string? refreshToken = Request.Cookies[CookieConstants.RefreshToken];
        return Ok(new TokenResponseSchema(refreshToken));
    }

    private void AddRefreshTokenToCookies(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            // Secure = true,
        };
        Response.Cookies.Append(CookieConstants.RefreshToken, refreshToken, cookieOptions);
    }
}
