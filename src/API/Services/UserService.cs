using API.Interfaces;
using API.Models.DbModels;
using API.Stores;
using Common.OperatingModels;

namespace API.Services;

public class UserService : IUserService
{
    private readonly UserStore _store;

    public UserService(UserStore store)
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
}