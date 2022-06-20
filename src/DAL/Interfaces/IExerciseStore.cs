using Common.DbModels;
using Common.OperatingModels;

namespace DAL.Interfaces;

public interface IExerciseStore
{
    public Task<GetEntityResult<List<Exercise>>> GetExercisesAsync();

    public Task<GetEntityResult<Exercise>> AddExerciseAsync(string description, string functionName);

    public Task<GetEntityResult<Exercise>> UpdateExerciseAsync(Exercise exercise);
}