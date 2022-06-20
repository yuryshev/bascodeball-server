using API.Services;
using Common.DbModels;
using Common.OperatingModels;
using DAL.Interfaces;
using Moq;
using Xunit;

namespace BLL.Tests.Services;

public class GetIdentityServiceTests
{
    private readonly Mock<IUserStore> _userStoreMock;

    public GetIdentityServiceTests()
    {
        _userStoreMock = new Mock<IUserStore>();
    }

    [Fact]
    public async void GetIdentity_CorrectData_CorrectIdentityReturned()
    {
        // Arrange
        var email = "test_email";
        var user = new User { Email = email, Role = "user"};

        _userStoreMock.Setup(s => s.GetUserByEmailAsync(email))
            .ReturnsAsync(GetEntityResult<User>.FromFound(user));

        var service = new GetIdentityService(_userStoreMock.Object);
        
        // Act
        var result = await service.GetIdentity(email);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Token", result.Entity.AuthenticationType);
        Assert.NotNull(result.Entity.NameClaimType);
        Assert.NotNull(result.Entity.RoleClaimType);
    }
    
    [Fact]
    public async void RegIdentity_CorrectData_CorrectIdentityReturned()
    {
        // Arrange
        var email = "test_email";
        var role = "test_role";
        var picture = "test_picture.png";
        var nickName = "test_nickname";
        var user = new User { Email = email, Role = role, Picture = picture, NickName = nickName};

        _userStoreMock.Setup(s => s.AddUserAsync(email, nickName, picture))
            .ReturnsAsync(GetEntityResult<User>.FromFound(user));

        var service = new GetIdentityService(_userStoreMock.Object);
        
        // Act
        var result = await service.RegIdentity(email, nickName, picture);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Token", result.Entity.AuthenticationType);
        Assert.NotNull(result.Entity.NameClaimType);
        Assert.NotNull(result.Entity.RoleClaimType);
    }
}