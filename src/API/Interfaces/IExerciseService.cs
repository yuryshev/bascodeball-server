using API.Models.DbModels;
using Common.OperatingModels;

namespace API.Interfaces;

public interface IExerciseService
{
    public Task<GetEntityResult<Exercise>> GetExerciseAsync(ICollection<Team> teams);
}