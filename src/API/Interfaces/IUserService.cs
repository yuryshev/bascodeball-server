using API.Models.DbModels;
using Common.OperatingModels;

namespace API.Interfaces;

public interface IUserService
{
    public Task<GetEntityResult<User>> GetUserByEmailAsync(string email);
}