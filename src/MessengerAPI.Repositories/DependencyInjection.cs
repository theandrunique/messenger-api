using Cassandra;
using Cassandra.Mapping;
using MessengerAPI.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, ConfigurationManager config)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("scylla")
            .WithDefaultKeyspace("messenger")
            .WithPort(9042)
            .WithRetryPolicy(new LoggingRetryPolicy(new DefaultRetryPolicy()))
            .Build();

        MappingConfiguration.Global.Define<ModelsMappings>();

        var session = cluster.Connect();

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
}