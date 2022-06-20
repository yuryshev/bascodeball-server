using BLL.Services;
using Common.DbModels;
using Common.OperatingModels;
using DAL.Interfaces;
using Moq;
using Xunit;

namespace BLL.Tests.Services;

public class ExerciseServiceTests
{
    private readonly Mock<IExerciseStore> _exerciseStoreMock;

    public ExerciseServiceTests()
    {
        _exerciseStoreMock = new Mock<IExerciseStore>();
    }

    [Fact]
    public async void GetExerciseAsync_CorrectResultReturned()
    {
        // Arrange
        var exercises = new List<Exercise> { new() { Title = "test_title1" }, new() {Title = "test_title2"} };

        _exerciseStoreMock.Setup(s => s.GetExercisesAsync())
            .ReturnsAsync(GetEntityResult<List<Exercise>>.FromFound(exercises));

        var service = new ExerciseService(_exerciseStoreMock.Object);

        // Act
        var result = await service.GetExerciseAsync();
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Entity, exercises);
    }
    
    
}