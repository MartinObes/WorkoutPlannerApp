using FluentAssertions;
using WorkoutPlanner.Application.WorkoutExcercises;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class WorkoutExcerciseLogicTest
{
    private WorkoutExcerciseLogic _workoutExcerciseLogic = null!;
    
    [TestInitialize]
    public void Initialize()
    {
        _workoutExcerciseLogic = new WorkoutExcerciseLogic();
    }
    
    [TestMethod]
    public void CreateWorkoutExcercise_ValidInput_ReturnsWorkoutExcercise()
    {
        // Arrange
        var name = "Bench Press";
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var sets = 3;
        var loadType = Enums.LoadType.Weight;
        var weight = 100;

        // Act
        var result = _workoutExcerciseLogic.createWorkoutExcercise(name, workoutId, excerciseId, reps, sets, loadType, weight);

        // Assert
        result.Should().NotBeNull();
        result.WorkoutId.Should().Be(workoutId);
        result.ExcerciseId.Should().Be(excerciseId);
        result.Reps.Should().Be(reps);
        result.Sets.Should().Be(sets);
        result.LoadType.Should().Be(loadType);
        result.Weight.Should().Be(weight);
        result.Percentage.Should().BeNull();
    }
    
    [TestMethod]
    public void CreateWorkoutExcercise_InvalidInput_ThrowsArgumentException()
    {
        // Arrange
        var name = "Bench Press";
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var sets = 3;
        var loadType = (Enums.LoadType)999; // Invalid load type

        // Act
        Action act = () => _workoutExcerciseLogic.createWorkoutExcercise(name, workoutId, excerciseId, reps, sets, loadType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid load type.");
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

        // Act
        Action act = () => _workoutExcerciseLogic.updateWorkoutExcercise(workoutExcercise!, name, workoutId, excerciseId, reps, sets, loadType, weight);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("WorkoutExcercise cannot be null.");
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

        // Act
        Action act = () => _workoutExcerciseLogic.updateWorkoutExcercise(workoutExcercise, name, workoutId, excerciseId, reps, sets, loadType);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid load type.");
    }

}