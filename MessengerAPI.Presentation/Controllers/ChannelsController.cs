using AutoMapper;
using MediatR;
using MessengerAPI.Application.Channels.Commands;
using MessengerAPI.Application.Channels.Commands.CreateMessage;
using MessengerAPI.Application.Channels.Commands.EditMessage;
using MessengerAPI.Application.Channels.Queries.GetChannels;
using MessengerAPI.Application.Channels.Queries.GetMessages;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using MessengerAPI.Presentation.Common;
using MessengerAPI.Presentation.Schemas.Channels;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Presentation.Controllers;

[Route("channels")]
public class ChannelsController : ApiController
{
    private readonly IMediator _mediator;

    public ChannelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetChannels()
    {
        var sub = User.GetUserId();

        var query = new GetChannelsQuery(sub);

        var result = await _mediator.Send(query);

        return result.Match(
            success => Ok(success),
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
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPost("{channelId}/messages")]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequestSchema schema, [FromRoute] Guid channelId)
    {
        var sub = User.GetUserId();

        var replyTo = schema.ReplyTo.HasValue ? new MessageId(schema.ReplyTo.Value) : null;

        var command = new CreateMessageCommand(
            sub,
            new ChannelId(channelId),
            schema.Text,
            replyTo,
            schema.Attachments);

        var result = await _mediator.Send(command);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpGet("{channelId}/messages")]
    public async Task<IActionResult> GetMessages([FromRoute] Guid channelId, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
    {
        var sub = User.GetUserId();

        var query = new GetMessagesQuery(sub, new ChannelId(channelId), offset, limit);

        var result = await _mediator.Send(query);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPut("{channelId}/messages/{messageId}")]
    public async Task<IActionResult> EditMessage(
        [FromBody] CreateMessageRequestSchema schema,
        [FromRoute] Guid channelId,
        [FromRoute] long messageId)
    {
        var sub = User.GetUserId();

        var replyTo = schema.ReplyTo.HasValue ? new MessageId(schema.ReplyTo.Value) : null;

        var command = new EditMessageCommand(
            new MessageId(messageId),
            sub,
            new ChannelId(channelId),
            schema.Text,
            replyTo,
            schema.Attachments);

        var result = await _mediator.Send(command);

        return result.Match(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}
