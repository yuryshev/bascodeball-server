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
    
    public async Task<GetEntityResult<User>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await this._dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return GetEntityResult<User>.FromNotFound();
            }
            return GetEntityResult<User>.FromFound(user);
        }
        catch
        {
            return GetEntityResult<User>.FromDbError();
        }
    }
    
    public async Task<GetEntityResult<User>> AddUserAsync(string email, string loginName, string picture)
    {
        try
        {
            var user = new User
            {
                Email = email,
                LoginName = loginName,
                Picture = picture,
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
    
    public async Task<GetEntityResult<User>> UpdateUserRatingAsync(string email, int rating)
    {
        try
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            dbUser.Rating = rating;
            
            this._dbContext.Users.Update(dbUser);
            await this._dbContext.SaveChangesAsync();
            return GetEntityResult<User>.FromFound(dbUser);
        }
        catch
        {
            return GetEntityResult<User>.FromDbError();
        }
    }
}