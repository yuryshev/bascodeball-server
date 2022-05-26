using API.Data;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;

namespace API.Stores;
public class UserStore
{
    private readonly PgDbContext _dbContext;

    public UserStore(PgDbContext dbContext)
    {
        this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<GetEntityResult<List<User>>> GetUsersAsync()
    {
        try
        {
            var users = await this._dbContext.Users.ToListAsync();
            if (users == null || users.Count == 0)
            {
                return GetEntityResult<List<User>>.FromNotFound();
            }
            return GetEntityResult<List<User>>.FromFound(users);
        }
        catch
        {
            return GetEntityResult<List<User>>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<User>> AddUserAsync(string email, string loginName)
    {
        try
        {
            var user = new User
            {
                Email = email,
                LoginName = loginName,
                Role = "user",
            };

            await this._dbContext.Users.AddAsync(user);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<User>.FromFound(user);
        }
        catch
        {
            return GetEntityResult<User>.FromDbError();
        }
    }
}