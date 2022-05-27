using API.Data;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;

namespace API.Stores;

public class ExerciseStore
{
    private readonly PgDbContext _dbContext;

    public ExerciseStore(PgDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<GetEntityResult<List<Exercise>>> GetExercisesAsync()
    {
        try
        {
            var exercises = await this._dbContext.Exercises.ToListAsync();
            if (exercises == null || exercises.Count == 0)
            {
                return GetEntityResult<List<Exercise>>.FromNotFound();
            }
            return GetEntityResult<List<Exercise>>.FromFound(exercises);
        }
        catch
        {
            return GetEntityResult<List<Exercise>>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<Exercise>> AddExerciseAsync(string description, string functionName)
    {
        try
        {
            var exercise = new Exercise
            {
                Description = description,
                FunctionName = functionName,
            };

            await this._dbContext.Exercises.AddAsync(exercise);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<Exercise>.FromFound(exercise);
        }
        catch
        {
            return GetEntityResult<Exercise>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<Exercise>> UpdateExerciseAsync(Exercise exercise)
    {
        try
        {
            this._dbContext.Exercises.Update(exercise);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<Exercise>.FromFound(exercise);
        }
        catch
        {
            return GetEntityResult<Exercise>.FromDbError();
        }
    }
}