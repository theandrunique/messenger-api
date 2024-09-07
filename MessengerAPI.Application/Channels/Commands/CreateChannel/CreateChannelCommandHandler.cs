using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Channel;

namespace MessengerAPI.Application.Channels.Commands;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand, ErrorOr<CreateChannelResult>>
{
    readonly IChannelRepository _channelRepository;

    public CreateChannelCommandHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<ErrorOr<CreateChannelResult>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (request.Recipients.Count == 1)
        {
            Channel? existedChannel = await _channelRepository.GetPrivateChannelAsync(request.Recipients[0], request.Sub);
            if (existedChannel is not null)
            {
                return new CreateChannelResult(existedChannel);
            }

            var newChannel = Channel.CreatePrivate(request.Recipients[0], request.Sub);

            await _channelRepository.AddAsync(newChannel);
            await _channelRepository.Commit();

            return new CreateChannelResult(newChannel);
        }

        var newGroupChannel = Channel.CreateGroup(request.Sub, request.Recipients, request.Title);

        await _channelRepository.AddAsync(newGroupChannel);
        await _channelRepository.Commit();

        return new CreateChannelResult(newGroupChannel);
    }
}
