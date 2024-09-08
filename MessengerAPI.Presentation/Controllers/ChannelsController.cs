using AutoMapper;
using MediatR;
using MessengerAPI.Application.Channels.Commands;
using MessengerAPI.Application.Channels.Queries.GetChannels;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using MessengerAPI.Presentation.Common;
using MessengerAPI.Presentation.Schemas.Channels;
using MessengerAPI.Presentation.Schemas.Common;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("channels")]
public class ChannelsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChannelsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetChannels()
    {
        var sub = User.GetUserId();

        var query = new GetChannelsQuery(sub);

        var result = await _mediator.Send(query);

        return result.Match(
            success => Ok(_mapper.Map<List<ChannelSchema>>(success)),
            errors => Problem(errors)
        );
    }
    [HttpPost]
    public async Task<IActionResult> CreateChannel([FromBody] CreateChannelRequestSchema schema)
    {
        var sub = User.GetUserId();

        var query = new CreateChannelCommand(
            sub,
            schema.Members.ConvertAll(m => new UserId(m)),
            schema.Title);
        
        var result = await _mediator.Send(query);

        return result.Match(
            success => Ok(_mapper.Map<ChannelSchema>(success)),
            errors => Problem(errors)
        );
    }
}
