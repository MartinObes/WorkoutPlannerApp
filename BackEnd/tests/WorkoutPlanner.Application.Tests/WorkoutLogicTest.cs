using FluentAssertions;
using WorkoutPlanner.Application.Workouts;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class WorkoutLogicTest
{
    private WorkoutLogic _workoutLogic = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _workoutLogic = new WorkoutLogic();
    }
    
    [TestMethod]
    public void CreateWorkout_WhenValidInput_ShouldCreateWorkout()
    {
        // Arrange
        string name = "Test Workout";
        Guid? coachId = Guid.NewGuid();
        
        // Act
        var workout = _workoutLogic.createWorkout(name, coachId);
        
        // Assert
        workout.Should().NotBeNull();
        workout.Name.Should().Be(name);
        workout.CoachId.Should().Be(coachId);
        
    }
    
    [TestMethod]
    public void CreateWorkout_WhenNameIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        string name = "";
        Guid? coachId = Guid.NewGuid();
        
        // Act 
        Action act = () => _workoutLogic.createWorkout(name, coachId);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be empty");
    }
    
    [TestMethod]
    public void UpdateWorkout_WhenValidInput_ShouldUpdateWorkout()
    {
        // Arrange
        var workout = _workoutLogic.createWorkout("Old Name", null);
        string newName = "New Name";
        
        // Act
        var updatedWorkout = _workoutLogic.updateWorkout(workout, newName);
        
        // Assert
        updatedWorkout.Should().NotBeNull();
        updatedWorkout.Name.Should().Be(newName);
    }

    [TestMethod]
    public void UpdateWorkout_WhenNameIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var workout = _workoutLogic.createWorkout("Old Name", null);
        string newName = "";
        
        // Act
        Action act = () => _workoutLogic.updateWorkout(workout, newName);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be empty");
    }
}