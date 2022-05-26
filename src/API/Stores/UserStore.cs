using API.Data;
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

    public async Task<GetEntityResult<List<string>>> GetUsersEmails()
    {
        try
        {
            var emails = await this._dbContext.Users.Select(x => x.Email).ToListAsync();
            return GetEntityResult<List<string>>.FromFound(emails);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}