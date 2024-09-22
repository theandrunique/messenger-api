using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(v => v.Value, v => new ChannelId(v));

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.OwnerId);

        builder.HasOne<Message>()
            .WithMany()
            .HasForeignKey(c => c.LastMessageId);

        builder.Property(c => c.LastMessageId)
            .HasConversion(v => v.Value, v => new MessageId(v))
            .IsRequired(false);

        builder.HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(c => c.ChannelId);

        builder.HasMany(c => c.Members)
            .WithMany(u => u.Channels);

        builder.OwnsMany(c => c.AdminIds, adminIdsBuilder =>
        {
            adminIdsBuilder.ToTable("ChannelAdminIds");

            adminIdsBuilder.Property(u => u.UserId)
                .HasColumnName("UserId")
                .HasConversion(v => v.Value, v => new UserId(v));

            adminIdsBuilder.HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId");

            adminIdsBuilder.WithOwner().HasForeignKey("ChannelId");

            adminIdsBuilder.HasKey("UserId", "ChannelId");
        });

        builder.OwnsMany(c => c.PinnedMessageIds, pmb =>
        {
            pmb.ToTable("PinnedMessageIds");

            pmb.Property(m => m.MessageId)
                .HasColumnName("MessageId")
                .HasConversion(v => v.Value, v => new MessageId(v));

            pmb.HasOne<Message>()
                .WithMany()
                .HasForeignKey("MessageId");

            pmb.WithOwner().HasForeignKey("ChannelId");

            pmb.HasKey("MessageId", "ChannelId");
        });
    }
}
