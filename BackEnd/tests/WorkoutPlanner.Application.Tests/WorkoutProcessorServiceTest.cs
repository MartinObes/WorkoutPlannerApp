using FluentAssertions;
using Moq;
using WorkoutPlanner.Application.Evaluations;
using WorkoutPlanner.Application.Services.WourkoutProcessorService;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Application.Workouts;
using WorkoutPlanner.Domain;
using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class WorkoutProcessorServiceTest
{
    private WorkoutProcessorService _workoutProcessorService = null!;
    private Mock<IEvaluationLogic> _evaluationLogicMock = null!;
    private Mock<IWorkoutLogic> _workoutLogicMock = null!;
    private Mock<IUserLogic> _userLogicMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _evaluationLogicMock = new Mock<IEvaluationLogic>();
        _workoutLogicMock = new Mock<IWorkoutLogic>();
        _userLogicMock = new Mock<IUserLogic>();
        _workoutProcessorService = new WorkoutProcessorService(
            _evaluationLogicMock.Object,
            _workoutLogicMock.Object,
            _userLogicMock.Object);
    }

    [TestMethod]
    public async Task ProcessWorkout_WithWeightLoadType_ReturnsProcessedExerciseWithCorrectWeight()
    {
        // Arrange
        string workoutName = "Morning Routine";
        string username = "john";
        Guid excerciseId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        var excercise = new Excercise { Id = excerciseId, Name = "Bench Press" };
        var workoutExcercise = new WorkoutExcercise(4, 6, LoadType.Weight, 100)
        {
            ExcerciseId = excerciseId,
            Excercise = excercise
        };
        var workout = new Workout { Name = workoutName, WorkoutExcercises = new List<WorkoutExcercise> { workoutExcercise } };
        var user = new User { Id = userId, Name = "John Doe" };

        _workoutLogicMock.Setup(w => w.GetWorkoutByName(workoutName)).ReturnsAsync(workout);
        _userLogicMock.Setup(u => u.GetUserByName(username)).ReturnsAsync(user);
        _evaluationLogicMock.Setup(e => e.GetEvaluationsByPlayerId(userId)).ReturnsAsync(new List<Evaluation>());

        // Act
        var result = await _workoutProcessorService.ProcessWorkout(workoutName, username);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].ExerciseName.Should().Be("Bench Press");
        result[0].Sets.Should().Be(4);
        result[0].Reps.Should().Be(6);
        result[0].CalculatedWeight.Should().Be(100);
        result[0].LoadType.Should().Be(LoadType.Weight);
        result[0].OriginalPercentage.Should().BeNull();
        _workoutLogicMock.Verify(w => w.GetWorkoutByName(workoutName), Times.Once);
        _userLogicMock.Verify(u => u.GetUserByName(username), Times.Once);
        _evaluationLogicMock.Verify(e => e.GetEvaluationsByPlayerId(userId), Times.Once);
    }

    [TestMethod]
    public async Task ProcessWorkout_WithPercentageLoadType_WhenEvaluationExists_ReturnsCalculatedWeight()
    {
        // Arrange
        string workoutName = "Strength Day";
        string username = "jane";
        Guid excerciseId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        var excercise = new Excercise { Id = excerciseId, Name = "Squat" };
        var workoutExcercise = new WorkoutExcercise(5, 5, LoadType.Percentage, null, 80)
        {
            ExcerciseId = excerciseId,
            Excercise = excercise
        };
        var workout = new Workout { Name = workoutName, WorkoutExcercises = new List<WorkoutExcercise> { workoutExcercise } };
        var user = new User { Id = userId, Name = "Jane Doe" };
        var evaluations = new List<Evaluation>
        {
            new() { Id = Guid.NewGuid(), PlayerId = userId, ExcerciseId = excerciseId, Reps = 5, Weight = 100, Date = DateTime.UtcNow }
        };

        _workoutLogicMock.Setup(w => w.GetWorkoutByName(workoutName)).ReturnsAsync(workout);
        _userLogicMock.Setup(u => u.GetUserByName(username)).ReturnsAsync(user);
        _evaluationLogicMock.Setup(e => e.GetEvaluationsByPlayerId(userId)).ReturnsAsync(evaluations);

        // Act
        var result = await _workoutProcessorService.ProcessWorkout(workoutName, username);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].ExerciseName.Should().Be("Squat");
        result[0].Sets.Should().Be(5);
        result[0].Reps.Should().Be(5);
        result[0].CalculatedWeight.Should().Be(80); // 80% of 100
        result[0].LoadType.Should().Be(LoadType.Percentage);
        result[0].OriginalPercentage.Should().Be(80);
        _evaluationLogicMock.Verify(e => e.GetEvaluationsByPlayerId(userId), Times.Once);
    }

    [TestMethod]
    public async Task ProcessWorkout_WithPercentageLoadType_WhenNoEvaluationExists_ReturnsNullWeightWithPercentage()
    {
        // Arrange
        string workoutName = "Leg Day";
        string username = "mike";
        Guid excerciseId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        var excercise = new Excercise { Id = excerciseId, Name = "Deadlift" };
        var workoutExcercise = new WorkoutExcercise(4, 4, LoadType.Percentage, null, 75)
        {
            ExcerciseId = excerciseId,
            Excercise = excercise
        };
        var workout = new Workout { Name = workoutName, WorkoutExcercises = new List<WorkoutExcercise> { workoutExcercise } };
        var user = new User { Id = userId, Name = "Mike Smith" };

        _workoutLogicMock.Setup(w => w.GetWorkoutByName(workoutName)).ReturnsAsync(workout);
        _userLogicMock.Setup(u => u.GetUserByName(username)).ReturnsAsync(user);
        _evaluationLogicMock.Setup(e => e.GetEvaluationsByPlayerId(userId)).ReturnsAsync(new List<Evaluation>());

        // Act
        var result = await _workoutProcessorService.ProcessWorkout(workoutName, username);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].ExerciseName.Should().Be("Deadlift");
        result[0].Sets.Should().Be(4);
        result[0].Reps.Should().Be(4);
        result[0].CalculatedWeight.Should().BeNull();
        result[0].LoadType.Should().Be(LoadType.Percentage);
        result[0].OriginalPercentage.Should().Be(75);
        _evaluationLogicMock.Verify(e => e.GetEvaluationsByPlayerId(userId), Times.Once);
    }

    [TestMethod]
    public async Task ProcessWorkout_WithPercentageLoadType_WhenMultipleEvaluationsExist_UsesLatestByDate()
    {
        // Arrange
        string workoutName = "Upper Body";
        string username = "sara";
        Guid excerciseId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        var excercise = new Excercise { Id = excerciseId, Name = "Overhead Press" };
        var workoutExcercise = new WorkoutExcercise(4, 6, LoadType.Percentage, null, 70)
        {
            ExcerciseId = excerciseId,
            Excercise = excercise
        };
        var workout = new Workout { Name = workoutName, WorkoutExcercises = new List<WorkoutExcercise> { workoutExcercise } };
        var user = new User { Id = userId, Name = "Sara Jones" };
        var evaluations = new List<Evaluation>
        {
            new() { Id = Guid.NewGuid(), PlayerId = userId, ExcerciseId = excerciseId, Reps = 5, Weight = 100, Date = new DateTime(2026, 1, 1) },
            new() { Id = Guid.NewGuid(), PlayerId = userId, ExcerciseId = excerciseId, Reps = 5, Weight = 120, Date = new DateTime(2026, 3, 1) }
        };

        _workoutLogicMock.Setup(w => w.GetWorkoutByName(workoutName)).ReturnsAsync(workout);
        _userLogicMock.Setup(u => u.GetUserByName(username)).ReturnsAsync(user);
        _evaluationLogicMock.Setup(e => e.GetEvaluationsByPlayerId(userId)).ReturnsAsync(evaluations);

        // Act
        var result = await _workoutProcessorService.ProcessWorkout(workoutName, username);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].CalculatedWeight.Should().Be(84); // 70% of 120 (latest eval), not 70 (70% of older 100)
        result[0].OriginalPercentage.Should().Be(70);
        _evaluationLogicMock.Verify(e => e.GetEvaluationsByPlayerId(userId), Times.Once);
    }

    [TestMethod]
    public async Task ProcessWorkout_WithMixedLoadTypes_ReturnsAllExercisesProcessedCorrectly()
    {
        // Arrange
        string workoutName = "Full Body";
        string username = "alex";
        Guid weightExcerciseId = Guid.NewGuid();
        Guid percentageExcerciseId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        var weightExcercise = new Excercise { Id = weightExcerciseId, Name = "Bench Press" };
        var percentageExcercise = new Excercise { Id = percentageExcerciseId, Name = "Squat" };
        var weightWorkoutExcercise = new WorkoutExcercise(3, 10, LoadType.Weight, 80)
        {
            ExcerciseId = weightExcerciseId,
            Excercise = weightExcercise
        };
        var percentageWorkoutExcercise = new WorkoutExcercise(4, 6, LoadType.Percentage, null, 85)
        {
            ExcerciseId = percentageExcerciseId,
            Excercise = percentageExcercise
        };
        var workout = new Workout { Name = workoutName, WorkoutExcercises = new List<WorkoutExcercise> { weightWorkoutExcercise, percentageWorkoutExcercise } };
        var user = new User { Id = userId, Name = "Alex Brown" };
        var evaluations = new List<Evaluation>
        {
            new() { Id = Guid.NewGuid(), PlayerId = userId, ExcerciseId = percentageExcerciseId, Reps = 5, Weight = 200, Date = DateTime.UtcNow }
        };

        _workoutLogicMock.Setup(w => w.GetWorkoutByName(workoutName)).ReturnsAsync(workout);
        _userLogicMock.Setup(u => u.GetUserByName(username)).ReturnsAsync(user);
        _evaluationLogicMock.Setup(e => e.GetEvaluationsByPlayerId(userId)).ReturnsAsync(evaluations);

        // Act
        var result = await _workoutProcessorService.ProcessWorkout(workoutName, username);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var benchPress = result.First(e => e.ExerciseName == "Bench Press");
        benchPress.CalculatedWeight.Should().Be(80);
        benchPress.LoadType.Should().Be(LoadType.Weight);
        benchPress.OriginalPercentage.Should().BeNull();

        var squat = result.First(e => e.ExerciseName == "Squat");
        squat.CalculatedWeight.Should().Be(170); // 85% of 200
        squat.LoadType.Should().Be(LoadType.Percentage);
        squat.OriginalPercentage.Should().Be(85);

        _evaluationLogicMock.Verify(e => e.GetEvaluationsByPlayerId(userId), Times.Once);
    }
}