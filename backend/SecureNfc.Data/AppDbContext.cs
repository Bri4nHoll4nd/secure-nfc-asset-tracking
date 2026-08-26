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
    public DbSet<V1User> Users => Set<V1User>();
    public DbSet<V1AssetLog> AssetLogs => Set<V1AssetLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<V1Tag>()
            .HasIndex(t => t.Uid)
            .IsUnique();

        modelBuilder.Entity<V1Asset>()
            .HasIndex(a => a.EntityCode)
            .IsUnique();

        modelBuilder.Entity<V1Asset>()
            .HasOne(a => a.Tag)
            .WithOne(t => t.Asset)
            .HasForeignKey<V1Tag>(t => t.AssetId);

        modelBuilder.Entity<V1Asset>()
            .HasMany(a => a.Logs)
            .WithOne(l => l.Asset)
            .HasForeignKey(l => l.AssetId);

        modelBuilder.Entity<V1User>()
            .HasMany(u => u.Assets)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);
    }
}