using System.Security.Claims;
using API.Controllers;
using API.Interfaces;
using Common.OperatingModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class AuthorizationControllerTests
{
    private readonly Mock<IIdentityService> _serviceMock;

    public AuthorizationControllerTests()
    {
        this._serviceMock = new Mock<IIdentityService>();
    }

    [Fact]
    public async void TokenAsync_CorrectEmail_TokenReturned()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, "someEmail"),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "someRole")
        };
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        
        this._serviceMock.Setup(x => x.GetIdentity(It.IsAny<string>()))
            .Returns(Task.FromResult(GetEntityResult<ClaimsIdentity>.FromFound(claimsIdentity)));

        var controller = new AuthorizationController(this._serviceMock.Object);

        var result = await controller.TokenAsync("someEmail");
        
        Assert.IsType<JsonResult>(result);
    }
    
    [Fact]
    public async void RegAsync_CorrectEmail_TokenReturned()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, "someEmail"),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "someRole")
        };
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        
        this._serviceMock.Setup(x => x.RegIdentity(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(GetEntityResult<ClaimsIdentity>.FromFound(claimsIdentity)));

        var controller = new AuthorizationController(this._serviceMock.Object);

        var result = await controller.RegistrationAsync("someEmail", "someLoginName");
        
        Assert.IsType<JsonResult>(result);
    }
}