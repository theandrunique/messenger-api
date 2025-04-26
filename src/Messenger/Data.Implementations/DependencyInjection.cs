using Cassandra;
using Cassandra.OpenTelemetry;
using Messenger.Data.Implementations.Channels;
using Messenger.Data.Implementations.Channels.Dto;
using Messenger.Data.Implementations.Channels.Queries;
using Messenger.Data.Implementations.Messages;
using Messenger.Data.Implementations.Messages.Queries;
using Messenger.Data.Implementations.Users;
using Messenger.Data.Implementations.Users.Queries;
using Messenger.Data.Implementations.VerificationCodes;
using Messenger.Data.Implementations.VerificationCodes.Queries;
using Messenger.Data.Interfaces.Channels;
using Messenger.Data.Interfaces.Users;
using Messenger.Data.Interfaces.VerificationCodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Messenger.Data.Implementations;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, ConfigurationManager config)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("scylla")
            .WithOpenTelemetryInstrumentation(o =>
            {
                o.BatchChildStatementLimit = 10;
                o.IncludeDatabaseStatement = true;
            })
            .WithPort(9042)
            .WithPoolingOptions(new PoolingOptions()
                .SetCoreConnectionsPerHost(HostDistance.Local, 2)
                .SetMaxConnectionsPerHost(HostDistance.Local, 10))
            .WithDefaultKeyspace("messenger")
            .WithRetryPolicy(new LoggingRetryPolicy(new DefaultRetryPolicy()))
            .Build();

        var session = cluster.Connect();

        session.UserDefinedTypes.Define(
            UdtMap.For<MessageInfoDto>("messageinfo"));

        services.AddSingleton<ISession>(s => session);

        // var cassandraSettings = new CassandraOptions();
        // config.Bind(nameof(CassandraOptions), cassandraSettings);

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        services.AddScoped<IMessageAckRepository, MessageAckRepository>();

        services.AddSingleton<UserQueries>();
        services.AddSingleton<ChannelByIdQueries>();
        services.AddSingleton<ChannelUserQueries>();
        services.AddSingleton<PrivateChannelQueries>();
        services.AddSingleton<MessageQueries>();
        services.AddSingleton<AttachmentQueries>();
        services.AddSingleton<SessionQueries>();
        services.AddSingleton<VerificationCodeQueries>();
        services.AddSingleton<MessageAckQueries>();
        services.AddSingleton<UsersByEmailQueries>();
        services.AddSingleton<UsersByUsernameQueries>();

        return services;
    }

    public static TracerProviderBuilder AddDataServicesInstrumentation(this TracerProviderBuilder builder)
    {
        return builder.AddSource(CassandraActivitySourceHelper.ActivitySourceName);
    }

    public static MeterProviderBuilder AddDataServicesInstrumentation(this MeterProviderBuilder builder)
    {
        return builder.AddCassandraInstrumentation();
    }
}
