using Cassandra.Mapping;
using MessengerAPI.Data.Tables;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data;

public class ModelsMappings : Mappings
{
    public ModelsMappings()
    {
        For<User>()
            .TableName("users")
            .PartitionKey(u => u.Id)
            .Column(u => u.Username, u => u.WithSecondaryIndex())
            .Column(u => u.Email, e => e.WithSecondaryIndex());

        For<Session>()
            .TableName("sessions")
            .PartitionKey(s => s.UserId)
            .ClusteringKey(s => s.Id)
            .Column(s => s.TokenId, t => t.WithSecondaryIndex());

        For<ChannelById>()
            .TableName("channels_by_id")
            .PartitionKey(c => c.ChannelId)
            .Column(c => c.ChannelType, cm => cm.WithDbType<int>());
        For<PrivateChannel>()
            .TableName("private_channels")
            .PartitionKey(p => p.UserId1, p => p.UserId2);
        For<ChannelUsers>()
            .TableName("channel_users_by_user_id")
            .Column(c => c.Images, cm => cm.WithFrozenValue())
            .PartitionKey(c => c.UserId);
        For<SavedMessagesChannel>()
            .TableName("saved_messages_channel")
            .PartitionKey(s => s.UserId);

        For<MessageByChannelId>()
            .TableName("messages")
            .PartitionKey(m => m.ChannelId)
            .ClusteringKey(m => m.Id, SortOrder.Descending);

        For<Attachment>()
            .TableName("attachments")
            .PartitionKey(a => a.ChannelId)
            .ClusteringKey(a => a.MessageId)
            .ClusteringKey(a => a.Id);
    }
}
