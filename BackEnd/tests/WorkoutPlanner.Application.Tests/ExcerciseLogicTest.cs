using FluentAssertions;
using WorkoutPlanner.Application.Excercises;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class ExcerciseLogicTest
{
    private ExcerciseLogic _excerciseLogic = null!;

    [TestInitialize]
    public void Initialize()
    {
        _excerciseLogic = new ExcerciseLogic();
    }
    
    [TestMethod]
    public void CreateExcercise_WhenValidInput_ReturnsExcercise()
    {
        // Arrange
        string name = "Push Up";
        
        // Act
        var result = _excerciseLogic.CreateExcercise(name);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Push Up");
        result.Id.Should().NotBeEmpty();
    }
    
    [TestMethod]
    public void CreateExcercise_WhenNameContainsSpecialCharacters_ThrowsArgumentException()
    {
        // Arrange
        string name = "Push@Up";
        
        // Act
        Action act = () => _excerciseLogic.CreateExcercise(name);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Excercise name cannot contain special characters.");
    }
    
    [TestMethod]
    public void CreateExcercise_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        string name = "   ";
        
        // Act
        Action act = () => _excerciseLogic.CreateExcercise(name);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Excercise name cannot be empty.");
    }
    
}