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
    
    public async Task<GetEntityResult<Exercise>> GetExerciseAsync(ICollection<Team> teams)
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
        var solvedExercises = teams.SelectMany(t => t.SolvedExercises);

        var exercise = exerciseList.FirstOrDefault(ex => !solvedExercises.Contains(ex));
        if (exercise == null)
        {
            return GetEntityResult<Exercise>.FromNotFound();
        }
        
        return GetEntityResult<Exercise>.FromFound(exercise);
    }
}