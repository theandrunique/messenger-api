using MediatR;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessage;
using MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessagePrivateChannel;

public class AddOrEditMessagePrivateChannelCommandHandler
    : IRequestHandler<AddOrEditMessagePrivateChannelCommand, ErrorOr<MessageSchema>>
{

    private readonly IMediator _mediator;

    public AddOrEditMessagePrivateChannelCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(AddOrEditMessagePrivateChannelCommand request, CancellationToken cancellationToken)
    {
        var channelRequestResult = await _mediator.Send(
            new GetOrCreatePrivateChannelCommand(request.Sub, request.userId),
            cancellationToken);

        if (channelRequestResult.IsError)
        {
            return channelRequestResult.Error;
        }

        var channel = channelRequestResult.Value;
        var channelId = long.Parse(channel.Id);

        return await _mediator.Send(
            new AddOrEditMessageCommand(request.MessageId, request.Sub, channelId, request.Content, request.Attachments),
            cancellationToken);
    }
}
