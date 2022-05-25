﻿using Api.Data;
using Api.Models;
using Api.Stores;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Tests.Stores;

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
    public async Task GetDispatchFileHeadersAsync_NoAppropriateData_ReturnsEmptyList()
    {
        // Arrange
        var user1 = new User
        {
            Email = "test_email1",
            LoginName = "test_login1",
        };
        
        var user2 = new User
        {
            Email = "test_email2",
            LoginName = "test_login2",
        };

        this._dbContext.AddRange(user1, user2);
        await this._dbContext.SaveChangesAsync();

        // Act
        var result = await this._store.GetUsersEmails();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GetEntityResult<List<string>>.ResultType.Found, result.Status);
        Assert.Equal(2, result.Entity.Count);
    }
}