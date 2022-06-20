using BLL.Interfaces;
using Common.DbModels;
using Common.OperatingModels;
using DAL.Interfaces;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserStore _store;

    public UserService(IUserStore store)
    {
        this._store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public async Task<GetEntityResult<User>> GetUserByEmailAsync(string email)
    {
        var userResult = await this._store.GetUserByEmailAsync(email);
        if (!userResult.IsSuccess)
        {
            if (userResult.Status == GetEntityResult<User>.ResultType.DatabaseError)
            {
                return GetEntityResult<User>.FromDbError();
            }

            return GetEntityResult<User>.FromNotFound();
        }

        var user = userResult.Entity;
        return GetEntityResult<User>.FromFound(user);
    }
    
    public async Task<GetEntityResult<User>> UpdateUserRatingAsync(string email, int rating)
    {
        var userResult = await this._store.UpdateUserRatingAsync(email, rating);
        if (!userResult.IsSuccess)
        {
            if (userResult.Status == GetEntityResult<User>.ResultType.DatabaseError)
            {
                return GetEntityResult<User>.FromDbError();
            }

            return GetEntityResult<User>.FromNotFound();
        }

        var user = userResult.Entity;
        return GetEntityResult<User>.FromFound(user);
    }

    public async Task<GetEntityResult<List<User>>> GetUsersRatingAsync()
    {
        var userResult = await this._store.GetUsersAsync();
        if (!userResult.IsSuccess)
        {
            if (userResult.Status == GetEntityResult<List<User>>.ResultType.DatabaseError)
            {
                return GetEntityResult<List<User>>.FromDbError();
            }

            return GetEntityResult<List<User>>.FromNotFound();
        }

        var sortedByRatingUserList = userResult.Entity.OrderByDescending(x => x.Rating).ToList();
        return GetEntityResult<List<User>>.FromFound(sortedByRatingUserList);
    }
}