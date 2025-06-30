using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cloud.Domain.Models;

namespace Cloud.Infrastructure.Data.Configuration;

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