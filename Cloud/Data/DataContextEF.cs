using Microsoft.EntityFrameworkCore;
using Cloud.Configuration;
using Cloud.Models;

namespace Cloud.Data;

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