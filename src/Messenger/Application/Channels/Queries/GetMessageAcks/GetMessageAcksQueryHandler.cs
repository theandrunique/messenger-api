using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.ValueObjects;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetMessageAcks;

public class GetMessageAcksQueryHandler : IRequestHandler<GetMessageAcksQuery, ErrorOr<GetMessageAcksQueryResult>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMessageAckRepository _messageAckRepository;

    public GetMessageAcksQueryHandler(
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IMessageAckRepository messageAckRepository)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _messageAckRepository = messageAckRepository;
    }

    public async Task<ErrorOr<GetMessageAcksQueryResult>> Handle(GetMessageAcksQuery request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel == null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        // we dont need to get an actual message id
        var acks = await _messageAckRepository.GetAcksByMessageId(channel.Id, request.MessageId);
        if (acks.Count == 0)
        {
            return ApiErrors.Channel.NoMessageAcksFound(request.MessageId);
        }

        var result = new Dictionary<ChannelMemberInfo, DateTimeOffset>();

        foreach (var ack in acks)
        {
            var member = channel.FindMember(ack.UserId);
            if (member != null)
            {
                result.Add(member, ack.Timestamp);
            }
        }

        return new GetMessageAcksQueryResult(result);
    }
}
