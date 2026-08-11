using SecureNfc.Data.Models.V1;
using Microsoft.EntityFrameworkCore;

namespace SecureNfc.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    public DbSet<V1Tag> Tags => Set<V1Tag>();
    public DbSet<V1Asset> Assets => Set<V1Asset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<V1Tag>()
            .HasIndex(t => t.Uid)
            .IsUnique();

        modelBuilder.Entity<V1Tag>()
            .HasIndex(t => t.EntityCode)
            .IsUnique();

        modelBuilder.Entity<V1Asset>()
            .HasOne(a => a.Tag)
            .WithOne()
            .HasForeignKey<V1Asset>(a => a.EntityCode)
            .HasPrincipalKey<V1Tag>(t => t.EntityCode);
    }
}