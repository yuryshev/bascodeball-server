using API.Models.DbModels;
using Common.OperatingModels;

namespace API.Interfaces;

public interface ITeamService
{
    public Task<GetEntityResult<Team>> CreateTeamAsync(Team inpuTeam);
}