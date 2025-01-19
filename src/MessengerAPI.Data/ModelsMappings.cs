using Cassandra.Mapping;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data;

public class ModelsMappings : Mappings
{
    public ModelsMappings()
    {
        For<User>()
            .TableName("users")
            .PartitionKey(u => u.Id)
            .Column(u => u.TerminateSessions, cm => cm.WithDbType<int>())
            .Column(u => u.Username, u => u.WithSecondaryIndex())
            .Column(u => u.Email, e => e.WithSecondaryIndex());

        For<Session>()
            .TableName("sessions")
            .PartitionKey(s => s.UserId)
            .ClusteringKey(s => s.Id)
            .Column(s => s.TokenId, t => t.WithSecondaryIndex());

        For<Message>()
            .TableName("messages")
            .PartitionKey(m => m.ChannelId)
            .Column(m => m.Type, cm => cm.WithDbType<int>())
            .Column(u => u.Author, cm => cm.Ignore())
            .ClusteringKey(m => m.Id, SortOrder.Descending);

        For<Attachment>()
            .TableName("attachments")
            .PartitionKey(a => a.ChannelId)
            .ClusteringKey(a => a.MessageId)
            .ClusteringKey(a => a.Id);
    }
}
