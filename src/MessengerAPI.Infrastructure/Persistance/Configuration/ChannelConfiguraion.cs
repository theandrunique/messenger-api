using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.OwnerId);

        builder.HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(c => c.ChannelId);

        builder.HasMany(c => c.Members).WithMany();

        builder.OwnsOne(c => c.Image);

        builder.OwnsMany(c => c.Admins, adminBuilder =>
        {
            adminBuilder.ToTable("ChannelAdmins");

            adminBuilder.HasOne<User>()
                .WithMany()
                .HasForeignKey(u => u.UserId);

            adminBuilder.WithOwner().HasForeignKey("ChannelId");

            adminBuilder.HasKey("UserId", "ChannelId");
        });

        builder.OwnsMany(c => c.PinnedMessageIds, pmb =>
        {
            pmb.ToTable("PinnedMessageIds");

            pmb.HasOne<Message>()
                .WithMany()
                .HasForeignKey(p => p.MessageId);

            pmb.WithOwner().HasForeignKey("ChannelId");

            pmb.HasKey("MessageId", "ChannelId");
        });
    }
}
