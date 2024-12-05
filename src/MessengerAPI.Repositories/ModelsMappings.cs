using Cassandra;
using Cassandra.Mapping;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.Relations;

namespace MessengerAPI.Repositories;

public class ModelsMappings : Mappings
{
    public ModelsMappings()
    {
        For<User>()
            .TableName("users")
            .PartitionKey(u => u.Id)
            .Column(u => u.Username, u => u.WithSecondaryIndex())
            .Column(u => u.Email, e => e.WithSecondaryIndex());

        For<Domain.Models.Entities.Session>()
            .TableName("sessions")
            .PartitionKey(s => s.UserId)
            .ClusteringKey(s => s.Id)
            .Column(s => s.TokenId, t => t.WithSecondaryIndex());

        For<Channel>()
            .TableName("channels")
            .PartitionKey(c => c.Id)
            .Column(c => c.Members, m => m.Ignore());

        For<ChannelMember>()
            .TableName("channel_members")
            .PartitionKey(c => c.ChannelId)
            .ClusteringKey(c => c.UserId)
            .Column(c => c.UserId, ui => ui.WithSecondaryIndex());

        For<Message>()
            .TableName("messages")
            .PartitionKey(m => m.ChannelId)
            .ClusteringKey(m => m.Id, SortOrder.Descending)
            .Column(m => m.Id, i => i.WithDbType<TimeUuid>())
            .Column(m => m.Attachments, a => a.Ignore());

        For<Attachment>()
            .TableName("attachments")
            .PartitionKey(a => a.ChannelId)
            .ClusteringKey(a => a.MessageId)
            .ClusteringKey(a => a.Id);
    }
}
