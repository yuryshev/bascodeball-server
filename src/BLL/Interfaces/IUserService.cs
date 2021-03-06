using Common.DbModels;
using Common.OperatingModels;

namespace BLL.Interfaces;

public interface IUserService
{
    public Task<GetEntityResult<User>> GetUserByEmailAsync(string email);

    public Task<GetEntityResult<User>> UpdateUserRatingAsync(string email, int rating);
    
    public Task<GetEntityResult<List<User>>> GetUsersRatingAsync();
}