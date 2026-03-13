using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkoutPlanner.Application.Workouts;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Repositories;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class WorkoutLogicTest
{
    private WorkoutLogic _workoutLogic = null!;
    private Mock<WorkoutRepository> _workoutRepositoryMock = null!;
    private readonly DbContextOptions<AppDbContext> _dummyOptions = new();
    
    [TestInitialize]
    public void Initialize()
    {
        var mockDbContext = new Mock<AppDbContext>(_dummyOptions);
        _workoutRepositoryMock = new Mock<WorkoutRepository>(MockBehavior.Strict, mockDbContext.Object);
        _workoutLogic = new WorkoutLogic(_workoutRepositoryMock.Object);
    }
    
    [TestMethod]
    public async Task CreateWorkout_WhenValidInput_ReturnsWorkout()
    {
        // Arrange
        string name = "Test Workout";
        Guid? coachId = Guid.NewGuid();
        _workoutRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Workout>()))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _workoutLogic.CreateWorkout(name, coachId);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.CoachId.Should().Be(coachId);
        result.Id.Should().NotBeEmpty();
        _workoutRepositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<Workout>(w => w.Name == result.Name && w.CoachId == result.CoachId)), Times.Once);
    }
    
    [TestMethod]
    public void CreateWorkout_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        string name = "";
        Guid? coachId = Guid.NewGuid();
        _workoutRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Workout>()))
            .Returns(Task.CompletedTask);
        
        // Act 
        Action act = () => _workoutLogic.CreateWorkout(name, coachId).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be empty. (Parameter 'name')");
        _workoutRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Workout>()), Times.Never);
    }
    
    [TestMethod]
    public void UpdateWorkout_WhenValidInput_ReturnsUpdatedWorkout()
    {
        // Arrange
        var workout = new Workout { Id = Guid.NewGuid(), Name = "Old Name", CoachId = null };
        string newName = "New Name";
        Guid? newCoachId = Guid.NewGuid();
        _workoutRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<Workout>()))
            .Verifiable();
        
        // Act
        var updatedWorkout = _workoutLogic.UpdateWorkout(workout, newName, newCoachId);
        
        // Assert
        updatedWorkout.Should().NotBeNull();
        updatedWorkout.Name.Should().Be(newName);
        updatedWorkout.CoachId.Should().Be(newCoachId);
        _workoutRepositoryMock.Verify(repo => repo.Update(
            It.Is<Workout>(w => w.Id == workout.Id && w.Name == newName && w.CoachId == newCoachId)), Times.Once);
    }

    [TestMethod]
    public void UpdateWorkout_WhenNullWorkout_ThrowsArgumentException()
    {
        // Arrange
        Workout workout = null!;
        _workoutRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<Workout>()))
            .Verifiable();
        
        // Act
        Action act = () => _workoutLogic.UpdateWorkout(workout, "New Name", Guid.NewGuid());
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Workout cannot be null.");
        _workoutRepositoryMock.Verify(repo => repo.Update(It.IsAny<Workout>()), Times.Never);
    }

    [TestMethod]
    public void UpdateWorkout_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var workout = new Workout { Id = Guid.NewGuid(), Name = "Old Name", CoachId = null };
        string name = " ";
        _workoutRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<Workout>()))
            .Verifiable();

        // Act
        Action act = () => _workoutLogic.UpdateWorkout(workout, name, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be empty. (Parameter 'name')");
        _workoutRepositoryMock.Verify(repo => repo.Update(It.IsAny<Workout>()), Times.Never);
    }

    [TestMethod]
    public void DeleteWorkout_WhenValidWorkout_CallsRepositoryDelete()
    {
        // Arrange
        var workout = new Workout { Id = Guid.NewGuid(), Name = "Workout A", CoachId = Guid.NewGuid() };
        _workoutRepositoryMock.Setup(repo => repo.Delete(workout)).Verifiable();

        // Act
        _workoutLogic.DeleteWorkout(workout);

        // Assert
        _workoutRepositoryMock.Verify(repo => repo.Delete(workout), Times.Once);
    }

    [TestMethod]
    public void DeleteWorkout_WhenNullWorkout_ThrowsArgumentNullException()
    {
        // Arrange
        Workout workout = null!;
        _workoutRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Workout>())).Verifiable();

        // Act
        Action act = () => _workoutLogic.DeleteWorkout(workout);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Workout cannot be null. (Parameter 'workout')");
        _workoutRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Workout>()), Times.Never);
    }

    [TestMethod]
    public async Task WorkoutExists_WhenWorkoutExists_ReturnsTrue()
    {
        // Arrange
        string name = "Leg Day";
        _workoutRepositoryMock.Setup(repo => repo.ExistsAsync(w => w.Name == name)).ReturnsAsync(true);

        // Act
        var result = await _workoutLogic.WorkoutExists(name);

        // Assert
        result.Should().BeTrue();
        _workoutRepositoryMock.Verify(repo => repo.ExistsAsync(w => w.Name == name), Times.Once);
    }

    [TestMethod]
    public async Task WorkoutExists_WhenWorkoutDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string name = "Leg Day";
        _workoutRepositoryMock.Setup(repo => repo.ExistsAsync(w => w.Name == name)).ReturnsAsync(false);

        // Act
        var result = await _workoutLogic.WorkoutExists(name);

        // Assert
        result.Should().BeFalse();
        _workoutRepositoryMock.Verify(repo => repo.ExistsAsync(w => w.Name == name), Times.Once);
    }

    [TestMethod]
    public async Task GetWorkoutByName_WhenWorkoutExists_ReturnsWorkout()
    {
        // Arrange
        string name = "Push Day";
        var workout = new Workout { Id = Guid.NewGuid(), Name = name, CoachId = Guid.NewGuid() };
        _workoutRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Workout, bool>>>(), null))
            .ReturnsAsync(workout);

        // Act
        var result = await _workoutLogic.GetWorkoutByName(name);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        _workoutRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Workout, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public void GetWorkoutByName_WhenWorkoutDoesNotExist_ThrowsArgumentNullException()
    {
        // Arrange
        string name = "Push Day";
        _workoutRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Workout, bool>>>(), null))
            .ReturnsAsync((Workout)null!);

        // Act
        Action act = () => _workoutLogic.GetWorkoutByName(name).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Workout cannot be null. (Parameter 'workout')");
        _workoutRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Workout, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task GetAllWorkouts_ReturnsListOfWorkoutsOrderedByName()
    {
        // Arrange
        var workouts = new List<Workout>
        {
            new() { Id = Guid.NewGuid(), Name = "Zeta", CoachId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), Name = "Alpha", CoachId = Guid.NewGuid() }
        };
        _workoutRepositoryMock.Setup(repo => repo.GetAllAsync(null, null)).ReturnsAsync(workouts);

        // Act
        var result = await _workoutLogic.GetAllWorkouts();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alpha");
        result[1].Name.Should().Be("Zeta");
        _workoutRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }

    [TestMethod]
    public async Task GetAllWorkouts_WhenNoWorkouts_ReturnsEmptyList()
    {
        // Arrange
        var workouts = new List<Workout>();
        _workoutRepositoryMock.Setup(repo => repo.GetAllAsync(null, null)).ReturnsAsync(workouts);

        // Act
        var result = await _workoutLogic.GetAllWorkouts();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _workoutRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }
}