using API.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PgDbContext : DbContext
{
    public PgDbContext(DbContextOptions<PgDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).HasMaxLength(128).IsRequired();
            entity.Property(e => e.LoginName).HasMaxLength(32).IsRequired();
        });
    }
}