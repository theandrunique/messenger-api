using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Presentation.Schemas.Auth;
using ErrorOr;
using MessengerAPI.Application.Auth.Commands.Login;

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
            success => Ok(success),
            errors => Problem(errors));
    }
}
