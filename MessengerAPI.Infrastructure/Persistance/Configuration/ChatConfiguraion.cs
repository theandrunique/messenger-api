using MessengerAPI.Domain.Chat.Entities;
using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.User;
using MessengerAPI.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(v => v.Value, v => new ChatId(v));

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
            .HasForeignKey(c => c.ChatId);
        
        builder.OwnsMany(c => c.MemberIds, memberIdsBuilder =>
        {
            memberIdsBuilder.ToTable("ChatMemberIds");

            memberIdsBuilder.Property(u => u.UserId)
                .HasColumnName("UserId")
                .HasConversion(v => v.Value, v => new UserId(v));
            
            memberIdsBuilder.HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId");

            memberIdsBuilder.WithOwner().HasForeignKey("ChatId");

            memberIdsBuilder.HasKey("UserId", "ChatId");
        });

        builder.OwnsMany(c => c.AdminIds, adminIdsBuilder =>
        {
            adminIdsBuilder.ToTable("ChatAdminIds");

            adminIdsBuilder.Property(u => u.UserId)
                .HasColumnName("UserId")
                .HasConversion(v => v.Value, v => new UserId(v));

            adminIdsBuilder.HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId");

            adminIdsBuilder.WithOwner().HasForeignKey("ChatId");

            adminIdsBuilder.HasKey("UserId", "ChatId");
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

            pmb.WithOwner().HasForeignKey("ChatId");

            pmb.HasKey("MessageId", "ChatId");
        });
    }
}
