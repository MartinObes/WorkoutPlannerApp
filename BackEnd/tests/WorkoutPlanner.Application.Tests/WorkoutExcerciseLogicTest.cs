using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using WorkoutPlanner.Application.WorkoutExcercises;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Repositories;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class WorkoutExcerciseLogicTest
{
    private WorkoutExcerciseLogic _workoutExcerciseLogic = null!;
    private Mock<WorkoutExcerciseRepository> _workoutExcerciseRepositoryMock = null!;
    private readonly DbContextOptions<AppDbContext> _dummyOptions = new();
    
    [TestInitialize]
    public void Initialize()
    {
        var mockDbContext = new Mock<AppDbContext>(_dummyOptions);
        _workoutExcerciseRepositoryMock = new Mock<WorkoutExcerciseRepository>(MockBehavior.Strict, mockDbContext.Object);
        _workoutExcerciseLogic = new WorkoutExcerciseLogic(_workoutExcerciseRepositoryMock.Object);
    }
    
    [TestMethod]
    public async Task CreateWorkoutExcercise_ValidInput_ReturnsWorkoutExcercise()
    {
        // Arrange
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var sets = 3;
        var loadType = Enums.LoadType.Weight;
        var weight = 100;
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _workoutExcerciseLogic.createWorkoutExcercise(workoutId, excerciseId, reps, sets, loadType, weight);

        // Assert
        result.Should().NotBeNull();
        result.WorkoutId.Should().Be(workoutId);
        result.ExcerciseId.Should().Be(excerciseId);
        result.Reps.Should().Be(reps);
        result.Sets.Should().Be(sets);
        result.LoadType.Should().Be(loadType);
        result.Weight.Should().Be(weight);
        result.Percentage.Should().BeNull();
        _workoutExcerciseRepositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<WorkoutExcercise>(we =>
                we.WorkoutId == workoutId &&
                we.ExcerciseId == excerciseId &&
                we.Reps == reps &&
                we.Sets == sets &&
                we.LoadType == loadType &&
                we.Weight == weight &&
                we.Percentage == null)), Times.Once);
    }
    
    [TestMethod]
    public void CreateWorkoutExcercise_InvalidInput_ThrowsArgumentException()
    {
        // Arrange
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var sets = 3;
        var loadType = (Enums.LoadType)999; // Invalid load type
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()))
            .Returns(Task.CompletedTask);

        // Act
        Action act = () => _workoutExcerciseLogic.createWorkoutExcercise(workoutId, excerciseId, reps, sets, loadType).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid load type.");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()), Times.Never);
    }

    [TestMethod]
    public void CreateWorkoutExcercise_WeightLoadWithoutWeight_ThrowsArgumentException()
    {
        // Arrange
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var sets = 3;
        var loadType = Enums.LoadType.Weight;
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()))
            .Returns(Task.CompletedTask);

        // Act
        Action act = () => _workoutExcerciseLogic.createWorkoutExcercise(workoutId, excerciseId, reps, sets, loadType).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Weight must be provided");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()), Times.Never);
    }

    [TestMethod]
    public void CreateWorkoutExcercise_PercentageLoadWithoutPercentage_ThrowsArgumentException()
    {
        // Arrange
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var sets = 3;
        var loadType = Enums.LoadType.Percentage;
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()))
            .Returns(Task.CompletedTask);

        // Act
        Action act = () => _workoutExcerciseLogic.createWorkoutExcercise(workoutId, excerciseId, reps, sets, loadType).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Percentage must be provided");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<WorkoutExcercise>()), Times.Never);
    }
    
    [TestMethod]
    public void UpdateWorkoutExcercise_ValidInput_ReturnsUpdatedWorkoutExcercise()
    {
        // Arrange
        var workoutExcercise = new WorkoutExcercise(3, 10, Enums.LoadType.Weight, 100);
        var name = "Bench Press";
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 12;
        var sets = 4;
        var loadType = Enums.LoadType.Weight;
        var weight = 110;
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<WorkoutExcercise>()))
            .Verifiable();

        // Act
        var result = _workoutExcerciseLogic.updateWorkoutExcercise(workoutExcercise, name, workoutId, excerciseId, reps, sets, loadType, weight);

        // Assert
        result.Should().NotBeNull();
        result.WorkoutId.Should().Be(workoutId);
        result.ExcerciseId.Should().Be(excerciseId);
        result.Reps.Should().Be(reps);
        result.Sets.Should().Be(sets);
        result.LoadType.Should().Be(loadType);
        result.Weight.Should().Be(weight);
        _workoutExcerciseRepositoryMock.Verify(repo => repo.Update(
            It.Is<WorkoutExcercise>(we =>
                we.Id == workoutExcercise.Id &&
                we.WorkoutId == workoutId &&
                we.ExcerciseId == excerciseId &&
                we.Reps == reps &&
                we.Sets == sets &&
                we.LoadType == loadType &&
                we.Weight == weight)), Times.Once);
    }
    
    [TestMethod]
    public void UpdateWorkoutExcercise_NullWorkoutExcercise_ThrowsArgumentException()
    {
        // Arrange
        WorkoutExcercise? workoutExcercise = null;
        var name = "Bench Press";
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 12;
        var sets = 4;
        var loadType = Enums.LoadType.Weight;
        var weight = 110;
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<WorkoutExcercise>()))
            .Verifiable();

        // Act
        Action act = () => _workoutExcerciseLogic.updateWorkoutExcercise(workoutExcercise!, name, workoutId, excerciseId, reps, sets, loadType, weight);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("WorkoutExcercise cannot be null.");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.Update(It.IsAny<WorkoutExcercise>()), Times.Never);
    }
    
    [TestMethod]
    public void UpdateWorkoutExcercise_InvalidInput_ThrowsArgumentException()
    {
        // Arrange
        var workoutExcercise = new WorkoutExcercise(3, 10, Enums.LoadType.Weight, 100);
        var name = "Bench Press";
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 12;
        var sets = 4;
        var loadType = (Enums.LoadType)999; // Invalid load type
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<WorkoutExcercise>()))
            .Verifiable();

        // Act
        Action act = () => _workoutExcerciseLogic.updateWorkoutExcercise(workoutExcercise, name, workoutId, excerciseId, reps, sets, loadType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid load type.");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.Update(It.IsAny<WorkoutExcercise>()), Times.Never);
    }

    [TestMethod]
    public void DeleteWorkoutExcercise_WhenValidWorkoutExcercise_CallsRepositoryDelete()
    {
        // Arrange
        var workoutExcercise = new WorkoutExcercise(3, 10, Enums.LoadType.Weight, 100)
        {
            WorkoutId = Guid.NewGuid(),
            ExcerciseId = Guid.NewGuid()
        };
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.Delete(workoutExcercise))
            .Verifiable();

        // Act
        _workoutExcerciseLogic.deleteWorkoutExcercise(workoutExcercise);

        // Assert
        _workoutExcerciseRepositoryMock.Verify(repo => repo.Delete(workoutExcercise), Times.Once);
    }

    [TestMethod]
    public void DeleteWorkoutExcercise_WhenNullWorkoutExcercise_ThrowsArgumentException()
    {
        // Arrange
        WorkoutExcercise workoutExcercise = null!;
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.Delete(It.IsAny<WorkoutExcercise>()))
            .Verifiable();

        // Act
        Action act = () => _workoutExcerciseLogic.deleteWorkoutExcercise(workoutExcercise);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("WorkoutExcercise cannot be null.");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.Delete(It.IsAny<WorkoutExcercise>()), Times.Never);
    }

    [TestMethod]
    public async Task GetWorkoutExcerciseById_WhenWorkoutExcerciseExists_ReturnsWorkoutExcercise()
    {
        // Arrange
        var id = Guid.NewGuid();
        var workoutExcercise = new WorkoutExcercise(3, 10, Enums.LoadType.Weight, 100)
        {
            Id = id,
            WorkoutId = Guid.NewGuid(),
            ExcerciseId = Guid.NewGuid()
        };

        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<WorkoutExcercise, bool>>>(), null))
            .ReturnsAsync(workoutExcercise);

        // Act
        var result = await _workoutExcerciseLogic.getWorkoutExcerciseById(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        _workoutExcerciseRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<WorkoutExcercise, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public void GetWorkoutExcerciseById_WhenWorkoutExcerciseDoesNotExist_ThrowsArgumentNullException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<WorkoutExcercise, bool>>>(), null))
            .ReturnsAsync((WorkoutExcercise)null!);

        // Act
        Action act = () => _workoutExcerciseLogic.getWorkoutExcerciseById(id).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("WorkoutExcercise cannot be null. (Parameter 'workoutExcercise')");
        _workoutExcerciseRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<WorkoutExcercise, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task GetAllWorkoutExcercises_WhenWorkoutExcercisesExist_ReturnsList()
    {
        // Arrange
        var workoutExcercises = new List<WorkoutExcercise>
        {
            new(3, 10, Enums.LoadType.Weight, 100) { WorkoutId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid() },
            new(4, 8, Enums.LoadType.Percentage, null, 70) { WorkoutId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid() }
        };

        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.GetAllAsync(null, null))
            .ReturnsAsync(workoutExcercises);

        // Act
        var result = await _workoutExcerciseLogic.getAllWorkoutExcercises();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _workoutExcerciseRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }

    [TestMethod]
    public async Task GetAllWorkoutExcercises_WhenNoWorkoutExcercises_ReturnsEmptyList()
    {
        // Arrange
        var workoutExcercises = new List<WorkoutExcercise>();
        _workoutExcerciseRepositoryMock
            .Setup(repo => repo.GetAllAsync(null, null))
            .ReturnsAsync(workoutExcercises);

        // Act
        var result = await _workoutExcerciseLogic.getAllWorkoutExcercises();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _workoutExcerciseRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }

}