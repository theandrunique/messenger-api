using MediatR;
using MessengerAPI.Application.Schemas.Common;
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
    [ProducesResponseType(typeof(UserPrivateSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var sub = User.GetUserId();

        var query = new GetMeQuery(sub);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
