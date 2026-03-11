using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkoutPlanner.Application.Excercises;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class ExcerciseLogicTest
{
    private ExcerciseLogic _excerciseLogic = null!;
    private Mock<IExcerciseRepository> _excerciseRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        var mockDbContextOptions = new DbContextOptions<AppDbContext>();
        var mockDbContext = new Mock<AppDbContext>(mockDbContextOptions);
        _excerciseRepositoryMock = new Mock<IExcerciseRepository>(MockBehavior.Strict, mockDbContext.Object);
        _excerciseLogic = new ExcerciseLogic(_excerciseRepositoryMock.Object);
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