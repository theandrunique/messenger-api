using Cassandra;
using Cassandra.OpenTelemetry;
using Messenger.Data.Scylla.Channels;
using Messenger.Data.Scylla.Channels.Dto;
using Messenger.Data.Scylla.Channels.Queries;
using Messenger.Data.Scylla.Messages;
using Messenger.Data.Scylla.Messages.Queries;
using Messenger.Data.Scylla.Users;
using Messenger.Data.Scylla.Users.Queries;
using Messenger.Data.Scylla.VerificationCodes;
using Messenger.Data.Scylla.VerificationCodes.Queries;
using Messenger.Data.Interfaces.Channels;
using Messenger.Data.Interfaces.Users;
using Messenger.Data.Interfaces.VerificationCodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Messenger.Options;
using Microsoft.Extensions.Options;

namespace Messenger.Data.Scylla;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<ISession>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>().Value.ScyllaDBOptions;

            var cluster = Cluster.Builder()
                .AddContactPoints(options.ContactPoints)
                .WithOpenTelemetryInstrumentation(o =>
                {
                    o.BatchChildStatementLimit = 10;
                    o.IncludeDatabaseStatement = true;
                })
                .WithPort(options.Port)
                .WithPoolingOptions(new PoolingOptions()
                    .SetCoreConnectionsPerHost(HostDistance.Local, 2)
                    .SetMaxConnectionsPerHost(HostDistance.Local, 10))
                .WithDefaultKeyspace(options.DefaultKeyspace)
                .WithRetryPolicy(new LoggingRetryPolicy(new DefaultRetryPolicy()))
                .Build();

            var session = cluster.Connect();

            session.UserDefinedTypes.Define(
                UdtMap.For<MessageInfoDto>("messageinfo"));

            return session;
        });

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
