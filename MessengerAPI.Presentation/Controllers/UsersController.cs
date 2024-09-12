using AutoMapper;
using MediatR;
using MessengerAPI.Application.Users.Queries;
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

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var sub = User.GetUserId();

        var query = new GetMeQuery(sub);

        var result = await _mediator.Send(query);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
