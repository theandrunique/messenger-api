using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Message");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(v => v.Value, v => new MessageId(v))
            .ValueGeneratedOnAdd();

        builder.HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId);
        builder.Property(m => m.SenderId)
            .HasConversion(v => v.Value, v => new UserId(v));
        
        builder.Property(m => m.ReplyTo)
            .HasConversion(v => v.Value, v => new MessageId(v));
        
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
                j => j.HasOne<FileData>().WithMany().HasForeignKey("FileDataId"),
                j => j.HasOne<Message>().WithMany().HasForeignKey("MessageId"),
                j =>
                {
                    j.HasKey("MessageId", "FileDataId");
                    j.Property<int>("MessageId");
                    j.Property<Guid>("FileDataId");
                });
    }
}
