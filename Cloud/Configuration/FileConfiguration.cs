using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cloud.Models;

namespace Cloud.Configuration;

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