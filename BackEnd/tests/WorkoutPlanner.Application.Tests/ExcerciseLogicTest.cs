using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkoutPlanner.Application.Excercises;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Repositories;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class ExcerciseLogicTest
{
    private ExcerciseLogic _excerciseLogic = null!;
    private Mock<ExcerciseRepository> _excerciseRepositoryMock = null!;
    private readonly DbContextOptions<AppDbContext> _dummyOptions = new();

    [TestInitialize]
    public void Initialize()
    {
        var mockDbContext = new Mock<AppDbContext>(_dummyOptions);
        _excerciseRepositoryMock = new Mock<ExcerciseRepository>(MockBehavior.Strict, mockDbContext.Object);
        _excerciseLogic = new ExcerciseLogic(_excerciseRepositoryMock.Object);
    }
    
    [TestMethod]
    public async Task CreateExcercise_WhenValidInput_ReturnsExcercise()
    {
        // Arrange
        string name = "Push Up";
        _excerciseRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Excercise>()))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _excerciseLogic.CreateExcercise(name);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Push Up");
        result.Id.Should().NotBeEmpty();
        _excerciseRepositoryMock.Verify(repo => repo.InsertAsync
            (It.Is<Excercise>(e => e.Id == result.Id && e.Name == result.Name)), Times.Once);
    }
    
    [TestMethod]
    public void CreateExcercise_WhenNameContainsSpecialCharacters_ThrowsArgumentException()
    {
        // Arrange
        string name = "Push@Up";
        
        // Act
        Action act = () => _excerciseLogic.CreateExcercise(name).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Excercise name cannot contain special characters.");
    }
    
    [TestMethod]
    public void CreateExcercise_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        string name = "   ";
        
        // Act
        Action act = () => _excerciseLogic.CreateExcercise(name).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Excercise name cannot be empty.");
    }
    
    [TestMethod]
    public void DeleteExcercise_WhenValidExcercise_CallsRepositoryDelete()
    {
        // Arrange
        var excercise = new Excercise { Id = Guid.NewGuid(), Name = "Squat" };
        _excerciseRepositoryMock.Setup(repo => repo.Delete(excercise)).Verifiable();
        
        // Act
        _excerciseLogic.DeleteExcercise(excercise);
        
        // Assert
        _excerciseRepositoryMock.Verify(repo => repo.Delete(excercise), Times.Once);
    }

    [TestMethod]
    public void DeleteExcercise_WhenNullExcercise_ThrowsArgumentNullException()
    {
        // Arrange
        var excercise = (Excercise)null!;
        _excerciseRepositoryMock.Setup(repo => repo.Delete(excercise)).Verifiable();
        
        // Act
        Action act = () => _excerciseLogic.DeleteExcercise(excercise);
        
        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Excercise cannot be null.*");
    }
    
}