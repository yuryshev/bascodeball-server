using Common.DbModels;
using Common.OperatingModels;

namespace API.Interfaces;

public interface IUserService
{
    public Task<GetEntityResult<User>> GetUserByEmailAsync(string email);

    public Task<GetEntityResult<User>> UpdateUserRating(string email, int rating);
    
    public Task<GetEntityResult<List<User>>> GetUsersRating();
}