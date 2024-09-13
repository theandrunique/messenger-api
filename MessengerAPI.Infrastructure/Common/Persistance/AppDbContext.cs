using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using MessengerAPI.Infrastructure.Persistance.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Infrastructure.Common.Persistance;

public class AppDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainInterceptor;

    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ReactionGroup> ReactionGroups { get; set; }
    public DbSet<FileData> Files { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
    {
        _publishDomainInterceptor = publishDomainEventsInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}
