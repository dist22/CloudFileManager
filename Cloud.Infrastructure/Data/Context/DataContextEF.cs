using Cloud.Domain.Models;
using Cloud.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Infrastructure.Data.Context;

public class DataContextEF(DbContextOptions<DataContextEF> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<FileRecord> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new FileConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}