using MediatR;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Infrastructure.Common.Persistance;

public class AppDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ReactionGroup> ReactionGroups { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        var entities = ChangeTracker.Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
