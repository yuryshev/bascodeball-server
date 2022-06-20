using System.Security.Claims;
using API.Controllers;
using BLL.Interfaces;
using Common.DbModels;
using Common.OperatingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class ExerciseControllerTests
{
    private readonly Mock<IExerciseService> _exerciseServiceMock;

    public ExerciseControllerTests()
    {
        _exerciseServiceMock = new Mock<IExerciseService>();
    }
    
    [Fact]
    public async void GetExerciseAsync_CorrectUser_UserReturned()
    {
        // Arrange
        var exercise = new Exercise {Title = "test_title"};
        
        this._exerciseServiceMock.Setup(x => x.GetExerciseAsync())
            .Returns(Task.FromResult(GetEntityResult<Exercise>.FromFound(exercise)));

        var controller = new ExerciseController(this._exerciseServiceMock.Object);

        // Act
        var result = await controller.GetExerciseAsync();
        
        // Assert
        Assert.IsType<JsonResult>(result);
        Assert.Equal(exercise, ((JsonResult) result).Value as Exercise);
    }
}