using API.Models.DbModels;
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
    
    /// <summary>
    /// Gets or sets the test data.
    /// </summary>
    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).HasMaxLength(128).IsRequired();
            entity.Property(e => e.LoginName).HasMaxLength(32).IsRequired();
            entity.Property(e => e.Picture).IsRequired();
            entity.Property(e => e.Rating).IsRequired();
        });
        
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.ToTable("Exercise");
            entity.HasKey(e => e.ExerciseId);
            entity.Property(e => e.Description).HasMaxLength(2000).IsRequired();
            entity.Property(e => e.FunctionName).HasMaxLength(32).IsRequired();
            entity.HasMany(e => e.TestingData).WithOne();
        });
        
        modelBuilder.Entity<TestData>(entity =>
        {
            entity.ToTable("TestData");
            entity.HasKey(e => e.TestDataId);
            entity.Property(e => e.InputData).HasMaxLength(128).IsRequired();
            entity.Property(e => e.ExpectedResult).HasMaxLength(128).IsRequired();
        });
        
        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Teams");
            entity.HasKey(e => e.TeamId);
            entity.HasMany(e => e.SolvedExercises).WithOne();
            entity.HasMany(e => e.Players).WithOne();
        });
    }
}