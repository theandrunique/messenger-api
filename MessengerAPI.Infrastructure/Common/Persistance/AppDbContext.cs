using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.User;
using MessengerAPI.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Infrastructure.Common.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ReactionGroup> ReactionGroups { get; set; }
    public DbSet<FileData> Files { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
