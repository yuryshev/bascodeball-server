using Common.DbModels;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PgDbContext : DbContext
{
    public PgDbContext(DbContextOptions<PgDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Gets or sets the exercises.
    /// </summary>
    public virtual DbSet<Exercise> Exercises { get; set; }
    
    /// <summary>
    /// Gets or sets the test data.
    /// </summary>
    public virtual DbSet<TestData> TestData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).HasMaxLength(128).IsRequired();
            entity.Property(e => e.NickName).HasMaxLength(32).IsRequired();
            entity.Property(e => e.Picture).IsRequired();
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.ConnectionId).IsRequired();
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.ToTable("Exercise");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(2000).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(32).IsRequired();
            entity.HasMany(e => e.Tests).WithOne();
        });

        modelBuilder.Entity<TestData>(entity =>
        {
            entity.ToTable("TestData");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InputData).HasMaxLength(128).IsRequired();
            entity.Property(e => e.ExpectedResult).HasMaxLength(128).IsRequired();
        });
    }
}