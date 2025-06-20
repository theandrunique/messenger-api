using Messenger.Domain.Entities;

namespace Messenger.Data.Interfaces.Channels;

public interface IMessageAckRepository
{
    Task UpsertMessageAckStatus(MessageAck messageAck);
    Task<List<MessageAck>> GetAcksByMessageId(long channelId, long messageId);
}
