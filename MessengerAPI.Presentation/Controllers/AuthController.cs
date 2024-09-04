using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Presentation.Schemas.Auth;
using MessengerAPI.Application.Auth.Commands.Login;
using MessengerAPI.Presentation.Common;

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
    public async Task<IActionResult> SignUp([FromForm] SignUpSchema schema)
    {
        var command = new RegisterCommand(schema.Username, schema.GlobalName, schema.Password);
    
        var registerResult = await _mediator.Send(command);

        return registerResult.Match(
            success => Ok(success),
            errors => Problem(errors));
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromForm] SignInSchema schema)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

        var command = new LoginCommand(schema.Login, schema.Password, userAgent, ipAddress);

        var loginResult = await _mediator.Send(command);

        return loginResult.Match(
            success => {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    // Secure = true,
                };

                Response.Cookies.Append(CookieConstants.RefreshToken, success.RefreshToken, cookieOptions);
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
}
