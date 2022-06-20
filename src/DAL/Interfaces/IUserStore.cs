using Common.DbModels;
using Common.OperatingModels;

namespace DAL.Interfaces;

public interface IUserStore
{
    public Task<GetEntityResult<List<User>>> GetUsersAsync();

    public Task<GetEntityResult<User>> GetUserByEmailAsync(string email);

    public Task<GetEntityResult<User>> AddUserAsync(string email, string loginName, string picture);

    public Task<GetEntityResult<User>> UpdateUserRatingAsync(string email, int rating);
}