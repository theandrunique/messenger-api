using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerAPI.Infrastructure.Persistance.Configuration;

public class FileConfiguration : IEntityTypeConfiguration<FileData>
{
    public void Configure(EntityTypeBuilder<FileData> builder)
    {
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.OwnerId);
    }
}