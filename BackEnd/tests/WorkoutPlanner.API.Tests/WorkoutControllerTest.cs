using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutPlanner.API.Controllers;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Workouts;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.API.Tests;

[TestClass]
public class WorkoutControllerTest
{
    private Mock<IWorkoutLogic> _workoutLogicMock = null!;
    private WorkoutController _controller = null!;

    [TestInitialize]
    public void SetUp()
    {
        _workoutLogicMock = new Mock<IWorkoutLogic>(MockBehavior.Strict);
        _controller = new WorkoutController(_workoutLogicMock.Object);
    }

    [TestMethod]
    public async Task GetAll_ReturnsAllWorkouts()
    {
        // Arrange
        var workouts = new List<Domain.Workout>
        {
            new Domain.Workout { Name = "Workout1" },
            new Domain.Workout { Name = "Workout2" }
        };
        _workoutLogicMock.Setup(logic => logic.GetAllWorkouts()).ReturnsAsync(workouts);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Workouts.Should().HaveCount(2);
        result.Workouts[0].Name.Should().Be("Workout1");
        result.Workouts[1].Name.Should().Be("Workout2");
        _workoutLogicMock.Verify(logic => logic.GetAllWorkouts(), Times.Once);
    }

    [TestMethod]
    public async Task Create_ReturnsCreatedWorkout()
    {
        // Arrange
        var workoutName = "NewWorkout";
        var coachId = Guid.NewGuid();
        var workoutExcersiesArgs = new List<CreateWorkoutExcerciseArgs>
        {
            new CreateWorkoutExcerciseArgs { ExcerciseId = Guid.NewGuid(), Reps = 10, Sets = 3, LoadType = Enums.LoadType.Weight, Weight = 100 },
            new CreateWorkoutExcerciseArgs { ExcerciseId = Guid.NewGuid(), Reps = 15, Sets = 4, LoadType = Enums.LoadType.Percentage, Percentage = 75 }
        };
        var createdWorkoutExcercises = workoutExcersiesArgs
            .Select(args => new WorkoutExcercise(args.Sets, args.Reps, args.LoadType, args.Weight, args.Percentage)).ToList();
        var createdWorkout = new Workout
        { Name = workoutName, CoachId = coachId, WorkoutExcercises = createdWorkoutExcercises };
        _workoutLogicMock.Setup(logic => logic.CreateWorkout(workoutName, coachId, workoutExcersiesArgs)).ReturnsAsync(createdWorkout);
        var request = new Models.CreateWorkoutRequestDto { Name = workoutName, CoachId = coachId, WorkoutExcercises = workoutExcersiesArgs };


        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(workoutName);
        result.CoachId.Should().Be(coachId);
        result.WorkoutExcercises.Should().HaveCount(2);
        result.WorkoutExcercises[0].Reps.Should().Be(10);
        result.WorkoutExcercises[0].Sets.Should().Be(3);
        result.WorkoutExcercises[1].Reps.Should().Be(15);
        result.WorkoutExcercises[1].Sets.Should().Be(4);
        _workoutLogicMock.Verify(logic => logic.CreateWorkout(workoutName, coachId, workoutExcersiesArgs), Times.Once);
    }

    [TestMethod]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        var workoutName = "WorkoutToDelete";
        _workoutLogicMock.Setup(logic => logic.DeleteWorkout(workoutName)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(workoutName);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _workoutLogicMock.Verify(logic => logic.DeleteWorkout(workoutName), Times.Once);
    }

    [TestMethod]
    public async Task GetByName_ReturnsWorkout()
    {
        // Arrange
        var workoutName = "ExistingWorkout";
        var workout = new Workout { Name = workoutName };
        _workoutLogicMock.Setup(logic => logic.GetWorkoutByName(workoutName)).ReturnsAsync(workout);

        // Act
        var result = await _controller.GetByName(workoutName);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(workoutName);
        _workoutLogicMock.Verify(logic => logic.GetWorkoutByName(workoutName), Times.Once);
    }

    [TestMethod]
    public async Task Update_ReturnsUpdatedWorkout()
    {
        // Arrange
        var workoutId = Guid.NewGuid();
        var workoutName = "UpdatedWorkout";
        var coachId = Guid.NewGuid();
        var updatedWorkout = new Workout { Id = workoutId, Name = workoutName, CoachId = coachId };
        _workoutLogicMock.Setup(logic => logic.UpdateWorkout(workoutId, workoutName, coachId)).ReturnsAsync(updatedWorkout);
        var request = new UpdateWorkoutRequestDto { WorkoutId = workoutId, Name = workoutName, CoachId = coachId };

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(workoutId);
        result.Name.Should().Be(workoutName);
        result.CoachId.Should().Be(coachId);
        _workoutLogicMock.Verify(logic => logic.UpdateWorkout(workoutId, workoutName, coachId), Times.Once);
    }

}