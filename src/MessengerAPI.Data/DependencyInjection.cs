using Cassandra;
using Cassandra.Mapping;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Queries;
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

        session.UserDefinedTypes.Define(
            UdtMap.For<Attachment>("attachment"),
            UdtMap.For<Image>("image"),
            UdtMap.For<MessageInfo>("messageinfo")
        );

        services.AddSingleton<ISession>(s => session);

        // var cassandraSettings = new CassandraOptions();
        // config.Bind(nameof(CassandraOptions), cassandraSettings);

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();

        services.AddSingleton<UserQueries>();
        services.AddSingleton<ChannelByIdQueries>();
        services.AddSingleton<ChannelUserQueries>();
        services.AddSingleton<PrivateChannelQueries>();
        services.AddSingleton<SavedMessagesChannelQueries>();
        services.AddSingleton<MessageQueries>();

        return services;
    }
}