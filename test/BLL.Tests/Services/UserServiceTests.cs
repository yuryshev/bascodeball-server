using BLL.Services;
using Common.DbModels;
using Common.OperatingModels;
using DAL.Interfaces;
using Moq;
using Xunit;

namespace BLL.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserStore> _userStoreMock;

    public UserServiceTests()
    {
        _userStoreMock = new Mock<IUserStore>();
    }

    [Fact]
    public async void GetUserByEmailAsync_CorrectEmail_UserReturned()
    {
        // Arrange
        var email = "test_email";
        var user = new User { Email = email };

        _userStoreMock.Setup(s => s.GetUserByEmailAsync(email)).ReturnsAsync(GetEntityResult<User>.FromFound(user));

        var service = new UserService(_userStoreMock.Object);

        // Act
        var result = await service.GetUserByEmailAsync(email);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(user, result.Entity);
    }
    
    [Fact]
    public async void UpdateUserRatingAsync_CorrectData_UserReturned()
    {
        // Arrange
        var email = "test_email";
        var rating = 30;
        var user = new User { Email = email, Rating = rating };

        _userStoreMock.Setup(s => s.UpdateUserRatingAsync(email, rating)).ReturnsAsync(GetEntityResult<User>.FromFound(user));

        var service = new UserService(_userStoreMock.Object);

        // Act
        var result = await service.UpdateUserRatingAsync(email, rating);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(user, result.Entity);
    }
    
    [Fact]
    public async void GetUsersRatingAsync_CorrectResultReturned()
    {
        // Arrange
        var user1 = new User { Rating = 10 }; // should be third
        var user2 = new User { Rating = 100 }; // should be first
        var user3 = new User { Rating = 0 }; // should be last
        var user4 = new User { Rating = 30 }; // should be second
        var users = new List<User>()
        {
            user1,
            user2,
            user3,
            user4
        };

        _userStoreMock.Setup(s => s.GetUsersAsync()).ReturnsAsync(GetEntityResult<List<User>>.FromFound(users));

        var service = new UserService(_userStoreMock.Object);

        // Act
        var result = await service.GetUsersRatingAsync();
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Entity.Count);
        Assert.Equal(user2, result.Entity[0]);
        Assert.Equal(user4, result.Entity[1]);
        Assert.Equal(user1, result.Entity[2]);
        Assert.Equal(user3, result.Entity[3]);
    }
}