using API.Interfaces;
using API.Models.DbModels;
using API.Stores;
using Common.OperatingModels;

namespace API.Services;

public class TeamService : ITeamService
{
    private readonly TeamStore _store;
    
    public TeamService(TeamStore store)
    {
        this._store = store ?? throw new ArgumentNullException(nameof(store));
    }
    
    public async Task<GetEntityResult<Team>> CreateTeamAsync(Team inputTeam)
    {
        var teamResult = await this._store.AddTeamAsync(inputTeam);
        if (!teamResult.IsSuccess)
        { 
            return GetEntityResult<Team>.FromDbError();
        }

        var team = teamResult.Entity;
        return GetEntityResult<Team>.FromFound(team);
    }
}