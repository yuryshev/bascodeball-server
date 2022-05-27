using API.Data;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;

namespace API.Stores;

public class TeamStore
{
    private readonly PgDbContext _dbContext;

    public TeamStore(PgDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<GetEntityResult<List<Team>>> GetTeamsAsync()
    {
        try
        {
            var teams = await this._dbContext.Teams.Where(t => t.IsDeleted == false).ToListAsync();
            if (teams == null || teams.Count == 0)
            {
                return GetEntityResult<List<Team>>.FromNotFound();
            }
            return GetEntityResult<List<Team>>.FromFound(teams);
        }
        catch
        {
            return GetEntityResult<List<Team>>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<Team>> AddTeamAsync(Team team)
    {
        try
        {
            await this._dbContext.Teams.AddAsync(team);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<Team>.FromFound(team);
        }
        catch
        {
            return GetEntityResult<Team>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<Team>> UpdateTeamAsync(Team team)
    {
        try
        {
            this._dbContext.Teams.Update(team);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<Team>.FromFound(team);
        }
        catch
        {
            return GetEntityResult<Team>.FromDbError();
        }
    }
}