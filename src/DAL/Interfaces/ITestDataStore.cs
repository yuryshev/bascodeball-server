using Common.DbModels;
using Common.OperatingModels;

namespace DAL.Interfaces;

public interface ITestDataStore
{
    public Task<GetEntityResult<List<TestData>>> GetTestDataListAsync(Guid exerciseId);

    public Task<GetEntityResult<List<TestData>>> GetTestDataByExerciseIdAsync(Guid exerciseId);

    public Task<GetEntityResult<TestData>> AddTestDataAsync(string inputData, string expectedResult);
}