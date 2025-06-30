using Cloud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cloud.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasKey(u => u.userId);

        builder
            .HasMany(u => u.files)
            .WithOne(f => f.user)
            .OnDelete(DeleteBehavior.Cascade);
    }
}