using DAL.Data;
using Common.DbModels;
using Common.OperatingModels;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Stores;

public class ExerciseStore : IExerciseStore
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
            var exercises = await this._dbContext.Exercises
                .Include(e => e.Tests)
                .ToListAsync();
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
    
    public async Task<GetEntityResult<Exercise>> AddExerciseAsync(string description, string title)
    {
        try
        {
            var exercise = new Exercise
            {
                Description = description,
                Title = title,
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