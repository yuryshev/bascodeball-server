using System.Security.Claims;
using API.Controllers;
using BLL.Interfaces;
using Common.DbModels;
using Common.DTOModels;
using Common.OperatingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
        
    public UserControllerTests()
    {
        this._userServiceMock = new Mock<IUserService>();
    }
    
    [Fact]
    public async void GetUserAsync_CorrectUser_UserReturned()
    {
        // Arrange
        var email = "test_email";
        var user = new User { Email = email, };
        var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, email) };
        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        var contextUser = new ClaimsPrincipal(claimsIdentity);
        var httpContext = new DefaultHttpContext { User = contextUser, };
        var controllerContext = new ControllerContext { HttpContext = httpContext, };
        
        this._userServiceMock.Setup(x => x.GetUserByEmailAsync(email))
            .Returns(Task.FromResult(GetEntityResult<User>.FromFound(user)));

        var controller = new UserController(this._userServiceMock.Object) { ControllerContext = controllerContext };

        // Act
        var result = await controller.GetUserAsync();
        
        // Assert
        Assert.IsType<JsonResult>(result);
        Assert.Equal(user, ((JsonResult) result).Value as User);
    }
    
    [Fact]
    public async void UpdateUserRatingAsync_ListUsers_RatingUpdated()
    {
        // Arrange
        var email = "test_email";
        var user = new User { Email = email, Rating = 0 };
        var userDTO = new UserDTO {Email = user.Email, Rating = user.Rating};
        var users = new List<UserDTO> { userDTO };
        
        this._userServiceMock.Setup(x => x.UpdateUserRatingAsync(user.Email, user.Rating))
            .Returns(Task.FromResult(GetEntityResult<User>.FromFound(user)));

        var controller = new UserController(this._userServiceMock.Object);

        // Act
        var result = await controller.UpdateUsersRatingAsync(users);
        
        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async void GetUserRatingAsync_CorrectData_UsersReturned()
    {
        // Arrange
        var user1 = new User { Email = "test_email1", Rating = 10 };
        var user2 = new User { Email = "test_email2", Rating = 20 };
        var users = new List<User> { user1, user2 };
        
        this._userServiceMock.Setup(x => x.GetUsersRatingAsync())
            .Returns(Task.FromResult(GetEntityResult<List<User>>.FromFound(users)));

        var controller = new UserController(this._userServiceMock.Object);

        // Act
        var result = await controller.GetUsersRatingAsync();
        
        // Assert
        Assert.IsType<JsonResult>(result);
        Assert.Equal(users, ((JsonResult) result).Value as List<User>);
    }
}