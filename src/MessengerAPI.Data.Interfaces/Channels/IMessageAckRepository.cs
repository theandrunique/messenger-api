using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Interfaces.Channels;

public interface IMessageAckRepository
{
    Task UpsertMessageAckStatus(MessageAck messageAck);
    Task<List<MessageAck>> GetAcksByMessageId(long channelId, long messageId);
}
