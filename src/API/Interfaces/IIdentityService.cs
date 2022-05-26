using System.Security.Claims;
using Common.OperatingModels;

namespace API.Interfaces;

public interface IIdentityService
{
    public Task<GetEntityResult<ClaimsIdentity>> GetIdentity(string inputEmail);
    
    public Task<GetEntityResult<ClaimsIdentity>> RegIdentity(string inputEmail, string inputLoginName);
}