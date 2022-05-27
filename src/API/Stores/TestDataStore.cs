using API.Data;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;

namespace API.Stores;

public class TestDataStore
{
    private readonly PgDbContext _dbContext;

    public TestDataStore(PgDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<GetEntityResult<List<TestData>>> GetTestDataListAsync(Guid exerciseId)
    {
        try
        {
            var testData = await this._dbContext.TestData.ToListAsync();
            if (testData == null || testData.Count == 0)
            {
                return GetEntityResult<List<TestData>>.FromNotFound();
            }
            return GetEntityResult<List<TestData>>.FromFound(testData);
        }
        catch
        {
            return GetEntityResult<List<TestData>>.FromDbError();
        }
    }

    public async Task<GetEntityResult<List<TestData>>> GetTestDataByExerciseIdAsync(Guid exerciseId)
    {
        try
        {
            var testData = await this._dbContext.Exercises
                .Where(e => e.ExerciseId == exerciseId)
                .Select(e => e.TestingData)
                .FirstOrDefaultAsync();
            if (testData == null || testData.Count == 0)
            {
                return GetEntityResult<List<TestData>>.FromNotFound();
            }
            return GetEntityResult<List<TestData>>.FromFound(testData.ToList());
        }
        catch
        {
            return GetEntityResult<List<TestData>>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<TestData>> AddTestDataAsync(string inputData, string expectedResult)
    {
        try
        {
            var testData = new TestData
            {
                InputData = inputData,
                ExpectedResult = expectedResult,
            };

            await this._dbContext.TestData.AddAsync(testData);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<TestData>.FromFound(testData);
        }
        catch
        {
            return GetEntityResult<TestData>.FromDbError();
        }
    }
}