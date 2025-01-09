using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Tables;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, ConfigurationManager config)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("scylla")
            .WithPort(9042)
            .WithPoolingOptions(new PoolingOptions()
                .SetCoreConnectionsPerHost(HostDistance.Local, 2)
                .SetMaxConnectionsPerHost(HostDistance.Local, 10))
            .WithDefaultKeyspace("messenger")
            .WithRetryPolicy(new LoggingRetryPolicy(new DefaultRetryPolicy()))
            .Build();

        MappingConfiguration.Global.Define<ModelsMappings>();

        var session = cluster.Connect();

        CreateTablesIfNotExists(session);

        services.AddSingleton<ISession>(s => session);

        // var cassandraSettings = new CassandraOptions();
        // config.Bind(nameof(CassandraOptions), cassandraSettings);

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();

        return services;
    }

    private static void CreateTablesIfNotExists(ISession session)
    {
        session.UserDefinedTypes.Define(
            UdtMap.For<Image>("image"),
            UdtMap.For<MessageInfo>("messageinfo"),
            UdtMap.For<MessageSenderInfo>("messagesenderinfo")
        );

        new Table<User>(session).CreateIfNotExists();
        new Table<Domain.Models.Entities.Session>(session).CreateIfNotExists();
        new Table<ChannelById>(session).CreateIfNotExists();
        new Table<PrivateChannel>(session).CreateIfNotExists();
        new Table<ChannelUsers>(session).CreateIfNotExists();
        new Table<Message>(session).CreateIfNotExists();
        new Table<Attachment>(session).CreateIfNotExists();
    }
}