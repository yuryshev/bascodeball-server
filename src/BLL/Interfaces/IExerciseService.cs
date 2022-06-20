using Common.DbModels;
using Common.OperatingModels;

namespace BLL.Interfaces;

public interface IExerciseService
{
    public Task<GetEntityResult<Exercise>> GetExerciseAsync();
}