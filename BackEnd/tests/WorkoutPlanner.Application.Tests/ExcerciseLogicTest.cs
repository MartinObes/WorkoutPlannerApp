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
    
    [TestMethod]
    public async Task GetExcerciseByName_WhenExcerciseExists_ReturnsExcercise()
    {
        // Arrange
        string name = "Deadlift";
        var excercise = new Excercise { Id = Guid.NewGuid(), Name = name };
        _excerciseRepositoryMock
            .Setup(repo => repo.GetAsync(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase), null))
            .ReturnsAsync(excercise);
        
        // Act
        var result = await _excerciseLogic.GetExcerciseByName(name);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Id.Should().Be(excercise.Id);
        _excerciseRepositoryMock.Verify(repo => repo.GetAsync(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase), null), Times.Once);
    }

    [TestMethod]
    public void GetExcerciseByName_WhenExcerciseDoesNotExist_ThrowsArgumentNullException()
    {
        // Arrange
        string name = "NonExistingExcercise";
        _excerciseRepositoryMock
            .Setup(repo => repo.GetAsync(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase), null))
            .ReturnsAsync((Excercise)null!);
        
        // Act
        Action act = () => _excerciseLogic.GetExcerciseByName(name).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Excercise cannot be null. (Parameter 'excercise')");
        _excerciseRepositoryMock.Verify(repo => repo.GetAsync(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase), null), Times.Once);
    }
    
    [TestMethod]
    public void GetExcerciseByName_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        string name = "   ";
        
        // Act
        Action act = () => _excerciseLogic.GetExcerciseByName(name).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Excercise name cannot be empty.");
    }
    
    [TestMethod]
    public async Task GetAllExcercises_ReturnsListOfExcercises()
    {
        // Arrange
        var excercises = new List<Excercise>
        {
            new Excercise { Id = Guid.NewGuid(), Name = "Bench Press" },
            new Excercise { Id = Guid.NewGuid(), Name = "Pull Up" }
        };
        _excerciseRepositoryMock
            .Setup(repo => repo.GetAllAsync(null, null))
            .ReturnsAsync(excercises);
        
        // Act
        var result = await _excerciseLogic.GetAllExcercises();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Bench Press");
        result[1].Name.Should().Be("Pull Up");
        _excerciseRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }

}