using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email)
            .IsUnique(true);

        builder.Property(u => u.Username)
            .HasMaxLength(50);

        builder.HasIndex(u => u.Username).IsUnique(true);

        builder.Property(u => u.Bio)
            .HasMaxLength(100);

        builder.Property(u => u.GlobalName)
            .HasMaxLength(50);

        builder.OwnsMany(u => u.Images, imageBuilder =>
        {
            imageBuilder.ToTable("UserImages");

            imageBuilder.HasKey("Id");

            imageBuilder.WithOwner().HasForeignKey();

            imageBuilder.HasIndex(i => i.Key).IsUnique(true);
        });

        builder.HasMany<Session>()
            .WithOne()
            .HasForeignKey(s => s.UserId);
    }
}

