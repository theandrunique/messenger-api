using MessengerAPI.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class ProfilePhotoConfiguration : IEntityTypeConfiguration<ProfilePhoto>
{
    public void Configure(EntityTypeBuilder<ProfilePhoto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.File)
            .WithOne();
    }
}
