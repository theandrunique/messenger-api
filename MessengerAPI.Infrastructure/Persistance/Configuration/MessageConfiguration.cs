using MessengerAPI.Domain.Chat.Entities;
using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(v => v.Value, v => new MessageId(v));

        builder.HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId);
        builder.Property(m => m.SenderId)
            .HasConversion(v => v.Value, v => new UserId(v));
        
        builder.Property(m => m.ReplyTo)
            .HasConversion(v => v.Value, v => new MessageId(v));
        
        builder.HasMany(m => m.Reactions)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "MessageReactions",
                j => j.HasOne<Reaction>().WithMany().HasForeignKey("ReactionId"),
                j => j.HasOne<Message>().WithMany().HasForeignKey("MessageId"),
                j =>
                {
                    j.HasKey("MessageId", "ReactionId");
                    j.Property<int>("MessageId");
                    j.Property<int>("ReactionId");
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