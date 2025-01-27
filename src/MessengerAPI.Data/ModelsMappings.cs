using Cassandra.Mapping;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data;

public class ModelsMappings : Mappings
{
    public ModelsMappings()
    {
        For<Attachment>()
            .TableName("attachments")
            .PartitionKey(a => a.ChannelId)
            .ClusteringKey(a => a.MessageId)
            .ClusteringKey(a => a.Id);
    }
}
