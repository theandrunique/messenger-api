using MediatR;
using Messenger.Application.WellKnown.Queries;
using Messenger.Presentation.Schemas.WellKnown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Presentation.Controllers;

[Route(".well-known")]
[AllowAnonymous]
public class WellKnownController : ApiController
{
    private readonly IMediator _mediator;

    public WellKnownController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("jwks.json")]
    [ProducesResponseType(typeof(JwkSetResponseSchema), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJwks()
    {
        var keys = await _mediator.Send(new GetJwkSetQuery());

        return Ok(new JwkSetResponseSchema(keys));
    }
}
