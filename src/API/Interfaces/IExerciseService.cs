using Common.DbModels;
using Common.OperatingModels;

namespace API.Interfaces;

public interface IExerciseService
{
    public Task<GetEntityResult<Exercise>> GetExerciseAsync();
}