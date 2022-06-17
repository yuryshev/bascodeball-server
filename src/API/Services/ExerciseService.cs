using API.Interfaces;
using API.Models.DbModels;
using API.Stores;
using Common.OperatingModels;

namespace API.Services;

public class ExerciseService : IExerciseService
{
    private readonly ExerciseStore _store;

    public ExerciseService(ExerciseStore store)
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