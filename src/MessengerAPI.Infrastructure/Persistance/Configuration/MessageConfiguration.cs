using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId);

        builder.OwnsMany(u => u.Reactions, urb =>
        {
            urb.ToTable("UserReactions");

            urb.WithOwner().HasForeignKey("MessageId");

            urb.HasOne(r => r.Reaction)
                .WithMany()
                .HasForeignKey("ReactionId");

            urb.HasOne<User>(r => r.User)
                .WithMany()
                .HasForeignKey("UserId");

            urb.HasKey("MessageId", "UserId");
        });

        builder.HasMany(m => m.Attachments)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "MessageAttachments",
                j => j.HasOne<Attachment>().WithMany().HasForeignKey("AttachmentId"),
                j => j.HasOne<Message>().WithMany().HasForeignKey("MessageId"),
                j =>
                {
                    j.HasKey("MessageId", "AttachmentId");
                    j.Property<long>("MessageId");
                    j.Property<long>("AttachmentId");
                });
    }
}
