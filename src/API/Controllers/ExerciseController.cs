using API.Interfaces;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class ExerciseController : Controller
{
    private readonly IExerciseService _exerciseService;
    public ExerciseController(IExerciseService exerciseService)
    {
        this._exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
    }
    
    [HttpGet("/get_exercise")]
    public async Task<IActionResult> GetExerciseAsync()
    {
        var exerciseResult = await _exerciseService.GetExerciseAsync();
        if (!exerciseResult.IsSuccess)
        {
            if (exerciseResult.Status == GetEntityResult<Exercise>.ResultType.NotFound)
            {
                return BadRequest(new { errorText = "Exercise not found." });
            }
            if (exerciseResult.Status == GetEntityResult<Exercise>.ResultType.DatabaseError)
            {
                return BadRequest(new { errorText = "Database error." });
            }
        }

        var exercise = exerciseResult.Entity;
        return new JsonResult(exercise);
    }
}