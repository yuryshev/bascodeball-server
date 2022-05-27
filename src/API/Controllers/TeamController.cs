using API.Interfaces;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class TeamController : Controller
{
    private ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        this._teamService = teamService;
    }
    
    [HttpPost("/addTeams")]
    public async Task<IActionResult> GetUserAsync(ICollection<Team> inputTeams)
    {
        var teams = new List<Team>();
        foreach (var inputTeam in inputTeams)
        {
            var teamResult = await _teamService.CreateTeamAsync(inputTeam);
            if (!teamResult.IsSuccess)
            {
                return BadRequest(new { errorText = "Database error." });
            }
            teams.Add(teamResult.Entity);
        }
        
        return new JsonResult(teams);
    }
}