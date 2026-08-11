using SecureNfc.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace SecureNfc.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Uid)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.EntityCode)
            .IsUnique();
    }
}