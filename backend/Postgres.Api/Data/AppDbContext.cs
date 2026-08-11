using Postgres.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Postgres.Api.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.EntityCode)
            .IsUnique();
    }
}