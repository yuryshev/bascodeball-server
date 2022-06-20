using DAL.Data;
using Common.DbModels;
using DAL.Stores;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DAL.Tests.Stores;

[Collection("Pg db collection")]
public class ExerciseStoreTests
{
    private readonly PgDbContext _dbContext;
    private readonly PgDbContextFixture _dbContextFixture;
    
    private readonly ExerciseStore _store;

    public ExerciseStoreTests(PgDbContextFixture dbContextFixture)
    {
        this._dbContext = dbContextFixture.GetPgDbContext();
        this._dbContextFixture = dbContextFixture;
        this._dbContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE ""Exercise"" CASCADE;");

        this._store = new ExerciseStore(this._dbContext);
    }
    
    [Fact]
    public void ExerciseStore_NullArgumentsPassed_ExceptionThrown()
    {
        // Arrange && Act && Assert
        Assert.Throws<ArgumentNullException>("dbContext", () => new ExerciseStore(null));

        _ = new ExerciseStore(this._dbContext);
    }
    
    [Fact]
    public async Task GetExercisesAsync_CorrectExerciseData_DbDataReturned()
    {
        // Arrange
        var exercise1 = new Exercise()
        {
            Title = "test_title1",
            Description = "test_description1",
            Tests = new List<TestData>()
        };
        
        var exercise2 = new Exercise()
        {
            Title = "test_title2",
            Description = "test_description2",
            Tests = new List<TestData>()
        };

        this._dbContext.AddRange(exercise1, exercise2);
        await this._dbContext.SaveChangesAsync();

        // Act
        var result = await this._store.GetExercisesAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GetEntityResult<List<Exercise>>.ResultType.Found, result.Status);
        Assert.Equal(2, result.Entity.Count);
    }
    
    [Fact]
    public async Task GetExercisesAsync_CorrectExerciseData_DbDataAdded()
    {
        // Arrange
        var exercise1 = new Exercise()
        {
            Title = "test_title1",
            Description = "test_description1"
        };
        
        var exercise2 = new Exercise()
        {
            Title = "test_title2",
            Description = "test_description2"
        };

        this._dbContext.Add(exercise1);
        await this._dbContext.SaveChangesAsync();

        // Act
        var result = await this._store.AddExerciseAsync(exercise2.Description, exercise2.Title);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GetEntityResult<Exercise>.ResultType.Found, result.Status);
        Assert.Equal(exercise2.Title, result.Entity.Title);
        Assert.Equal(exercise2.Description, result.Entity.Description);
    }
}