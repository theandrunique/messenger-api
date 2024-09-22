using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(v => v.Value, v => new UserId(v));

        builder.Property(u => u.Username)
            .HasMaxLength(50);
        builder.HasIndex(u => u.Username).IsUnique(true);

        builder.Property(u => u.Bio)
            .HasMaxLength(100);

        builder.Property(u => u.GlobalName)
            .HasMaxLength(50);

        builder.HasMany(u => u.ProfilePhotos)
            .WithOne()
            .HasForeignKey(p => p.UserId);

        builder.HasMany(u => u.Sessions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        ConfigureEmails(builder);
        ConfigurePhones(builder);
    }

    public void ConfigureEmails(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Emails, emailsBuilder =>
        {
            emailsBuilder.WithOwner().HasForeignKey("UserId");

            emailsBuilder.Property("Id");
            emailsBuilder.HasKey("Id");

            emailsBuilder.HasIndex(e => e.Data).IsUnique(true);
        });
    }

    public void ConfigurePhones(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Phones, phonesBuilder =>
        {
            phonesBuilder.WithOwner().HasForeignKey("UserId");

            phonesBuilder.Property("Id");
            phonesBuilder.HasKey("Id");

            phonesBuilder.HasIndex(e => e.Data).IsUnique(true);
        });
    }
}