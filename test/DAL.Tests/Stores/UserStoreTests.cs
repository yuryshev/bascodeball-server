using DAL.Data;
using Common.DbModels;
using DAL.Stores;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DAL.Tests.Stores;

[Collection("Pg db collection")]
public class UserStoreTests
{
    private readonly PgDbContext _dbContext;
    private readonly PgDbContextFixture _dbContextFixture;
    
    private readonly UserStore _store;

    public UserStoreTests(PgDbContextFixture dbContextFixture)
    {
        this._dbContext = dbContextFixture.GetPgDbContext();
        this._dbContextFixture = dbContextFixture;
        this._dbContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE ""User"" CASCADE;");

        this._store = new UserStore(this._dbContext);
    }
    
    [Fact]
    public void UserStore_NullArgumentsPassed_ExceptionThrown()
    {
        // Arrange && Act && Assert
        Assert.Throws<ArgumentNullException>("dbContext", () => new UserStore(null));

        _ = new UserStore(this._dbContext);
    }
    
    [Fact]
    public async Task GetUsersAsync_CorrectUsersData_DbDataAdded()
    {
        // Arrange
        var user1 = new User
        {
            Email = "test_email1",
            NickName = "test_login1",
            Picture = "picture1.png",
            Role = "user",
            ConnectionId = String.Empty
        };
        
        var user2 = new User
        {
            Email = "test_email2",
            NickName = "test_login2",
            Picture = "picture2.png",
            Role = "user",
            ConnectionId = String.Empty
        };

        this._dbContext.AddRange(user1, user2);
        await this._dbContext.SaveChangesAsync();

        // Act
        var result = await this._store.GetUsersAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GetEntityResult<List<User>>.ResultType.Found, result.Status);
        Assert.Equal(2, result.Entity.Count);
    }
    
    [Fact]
    public async Task AddUserAsync_CorrectUsersData_DbDataAdded()
    {
        // Arrange
        var user1 = new User
        {
            Email = "test_email1",
            NickName = "test_login1",
            Picture = "picture1.png",
            Role = "user",
            ConnectionId = String.Empty
        };
        
        var user2 = new User
        {
            Email = "test_email2",
            NickName = "test_login2",
            Picture = "picture2.png",
            Role = "user",
            ConnectionId = String.Empty
        };

        this._dbContext.AddRange(user1);
        await this._dbContext.SaveChangesAsync();

        // Act
        var result = await this._store.AddUserAsync(user2.Email, user2.NickName, user2.Picture);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GetEntityResult<User>.ResultType.Found, result.Status);
        Assert.Equal(user2.Email, result.Entity.Email);
        Assert.Equal(user2.NickName, result.Entity.NickName);
    }
}