using Messenger.Domain.Channels;

namespace Messenger.Domain.Data.Messages;

public interface IMessageAckRepository
{
    Task UpsertMessageAckStatus(MessageAck messageAck);
    Task<List<MessageAck>> GetAcksByMessageId(long channelId, long messageId);
}
