using BLL.Interfaces;
using Common.DbModels;
using Common.OperatingModels;
using DAL.Interfaces;

namespace BLL.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseStore _store;

    public ExerciseService(IExerciseStore store)
    {
        this._store = store ?? throw new ArgumentNullException(nameof(store));
    }
    
    public async Task<GetEntityResult<Exercise>> GetExerciseAsync()
    {
        var exercisesResult = await this._store.GetExercisesAsync();
        if (!exercisesResult.IsSuccess)
        {
            if (exercisesResult.Status == GetEntityResult<List<Exercise>>.ResultType.DatabaseError)
            {
                return GetEntityResult<Exercise>.FromDbError();
            }

            return GetEntityResult<Exercise>.FromNotFound();
        }

        var exerciseList = exercisesResult.Entity;
        var exercise = exerciseList.ElementAt(new Random().Next(exerciseList.Count));
        
        return GetEntityResult<Exercise>.FromFound(exercise);
    }
}