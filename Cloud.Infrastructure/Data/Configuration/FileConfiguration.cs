using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cloud.Domain.Models;

namespace Cloud.Infrastructure.Data.Configuration;

public class FileConfiguration : IEntityTypeConfiguration<FileRecord>
{
    public void Configure(EntityTypeBuilder<FileRecord> builder)
    {
        builder.HasKey(f => f.fileId);

        builder
            .HasOne(f => f.user)
            .WithMany(u => u.files)
            .HasForeignKey(f => f.userId);

    }
}