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
    public DbSet<V1Log> Logs => Set<V1Log>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<V1Tag>()
            .HasIndex(t => t.Uid)
            .IsUnique();

        modelBuilder.Entity<V1Tag>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Tag_OnlyOneOwner",
                """
                NOT ("AssetId" IS NOT NULL AND "UserId" IS NOT NULL)
                """
            ));
                
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

        modelBuilder.Entity<V1User>()
            .HasOne(u => u.Tag)
            .WithOne(t => t.User)
            .HasForeignKey<V1Tag>(t => t.UserId);

        modelBuilder.Entity<V1User>()
            .HasMany(u => u.AssetLogs)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);
    }
}