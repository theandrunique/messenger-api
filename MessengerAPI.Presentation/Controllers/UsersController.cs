using AutoMapper;
using MediatR;
using MessengerAPI.Application.Users.Queries;
using MessengerAPI.Presentation.Common;
using MessengerAPI.Presentation.Schemas.Common;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("users")]
public class UsersController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var sub = User.GetUserId();

        var query = new GetMeQuery(sub);

        var result = await _mediator.Send(query);

        return result.Match(
            success => Ok(_mapper.Map<UserPrivateSchema>(success)),
            errors => Problem(errors)
        );
    }
}
