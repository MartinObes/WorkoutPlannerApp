using FluentAssertions;
using Moq;
using WorkoutPlanner.API.Controllers;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.WorkoutExcercises;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Tests;

[TestClass]
public class WorkoutExcerciseControllerTest
{
    private Mock<IWorkoutExcerciseLogic> _workoutExcerciseLogicMock = null!;
    private WorkoutExcerciseController _controller = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _workoutExcerciseLogicMock = new Mock<IWorkoutExcerciseLogic>(MockBehavior.Strict);
        _controller = new WorkoutExcerciseController(_workoutExcerciseLogicMock.Object);
    }
    
    [TestMethod]
    public async Task GetAll_ReturnsWorkoutExcercisesResponseDto()
    {
        // Arrange
        var workoutExcercises = new List<WorkoutExcercise>
        {
            new WorkoutExcercise(3, 10, Enums.LoadType.Weight, weight: 100)
            {
                Id = Guid.NewGuid(),
                WorkoutId = Guid.NewGuid(),
                ExcerciseId = Guid.NewGuid(),
                Excercise = new Excercise { Name = "Bench Press" }
            },
            new WorkoutExcercise(2, 5, Enums.LoadType.Percentage, percentage: 80)
            {
                Id = Guid.NewGuid(),
                WorkoutId = Guid.NewGuid(),
                ExcerciseId = Guid.NewGuid(),
                Excercise = new Excercise { Name = "Squat" }
            }
        };
        _workoutExcerciseLogicMock.Setup(logic => logic.getAllWorkoutExcercises()).ReturnsAsync(workoutExcercises);
        
        // Act
        var result = await _controller.GetAll();
        
        // Assert
        result.Should().NotBeNull();
        result.WorkoutExcercises.Should().HaveCount(2);
        result.WorkoutExcercises[0].Id.Should().Be(workoutExcercises[0].Id);
        result.WorkoutExcercises[0].WorkoutId.Should().Be(workoutExcercises[0].WorkoutId);
        result.WorkoutExcercises[0].ExcerciseId.Should().Be(workoutExcercises[0].ExcerciseId);
        result.WorkoutExcercises[0].ExcerciseName.Should().Be("Bench Press");
        result.WorkoutExcercises[0].Reps.Should().Be(workoutExcercises[0].Reps);
        result.WorkoutExcercises[0].Sets.Should().Be(workoutExcercises[0].Sets);
        result.WorkoutExcercises[0].LoadType.Should().Be(workoutExcercises[0].LoadType);
        result.WorkoutExcercises[0].Weight.Should().Be(workoutExcercises[0].Weight);
        result.WorkoutExcercises[0].Percentage.Should().Be(workoutExcercises[0].Percentage);
        result.WorkoutExcercises[1].Id.Should().Be(workoutExcercises[1].Id);
        result.WorkoutExcercises[1].WorkoutId.Should().Be(workoutExcercises[1].WorkoutId);
        result.WorkoutExcercises[1].ExcerciseId.Should().Be(workoutExcercises[1].ExcerciseId);
        result.WorkoutExcercises[1].ExcerciseName.Should().Be("Squat");
        result.WorkoutExcercises[1].Reps.Should().Be(workoutExcercises[1].Reps);
        result.WorkoutExcercises[1].Sets.Should().Be(workoutExcercises[1].Sets);
        result.WorkoutExcercises[1].LoadType.Should().Be(workoutExcercises[1].LoadType);
        result.WorkoutExcercises[1].Weight.Should().Be(workoutExcercises[1].Weight);
        result.WorkoutExcercises[1].Percentage.Should().Be(workoutExcercises[1].Percentage);
        _workoutExcerciseLogicMock.Verify(logic => logic.getAllWorkoutExcercises(), Times.Once);
    }
    
    [TestMethod]
    public async Task GetAll_ReturnsEmptyList_WhenNoWorkoutExcercises()
    {
        // Arrange
        _workoutExcerciseLogicMock.Setup(logic => logic.getAllWorkoutExcercises()).ReturnsAsync(new List<WorkoutExcercise>());
        
        // Act
        var result = await _controller.GetAll();
        
        // Assert
        result.Should().NotBeNull();
        result.WorkoutExcercises.Should().BeEmpty();
        _workoutExcerciseLogicMock.Verify(logic => logic.getAllWorkoutExcercises(), Times.Once);
    }
    
    [TestMethod]
    public async Task GetById_ReturnsWorkoutExcerciseResponseDto()
    {
        // Arrange
        var workoutExcerciseId = Guid.NewGuid();
        var workoutExcercise = new WorkoutExcercise(4, 6, Enums.LoadType.Weight, weight: 90)
        {
            Id = workoutExcerciseId,
            WorkoutId = Guid.NewGuid(),
            ExcerciseId = Guid.NewGuid(),
            Excercise = new Excercise { Name = "Deadlift" }
        };
        _workoutExcerciseLogicMock.Setup(logic => logic.getWorkoutExcerciseById(workoutExcerciseId)).ReturnsAsync(workoutExcercise);
        var request = new GetWorkoutExcerciseByIdRequestDto { WorkoutExcerciseId = workoutExcerciseId };

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(workoutExcercise.Id);
        result.WorkoutId.Should().Be(workoutExcercise.WorkoutId);
        result.ExcerciseId.Should().Be(workoutExcercise.ExcerciseId);
        result.ExcerciseName.Should().Be("Deadlift");
        result.Reps.Should().Be(workoutExcercise.Reps);
        result.Sets.Should().Be(workoutExcercise.Sets);
        result.LoadType.Should().Be(workoutExcercise.LoadType);
        result.Weight.Should().Be(workoutExcercise.Weight);
        result.Percentage.Should().Be(workoutExcercise.Percentage);
        _workoutExcerciseLogicMock.Verify(logic => logic.getWorkoutExcerciseById(workoutExcerciseId), Times.Once);
    }

    [TestMethod]
    public async Task Update_ReturnsUpdatedWorkoutExcerciseResponseDto()
    {
        // Arrange
        var workoutExcerciseId = Guid.NewGuid();
        var workoutId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var request = new UpdateWorkoutExcerciseRequestDto
        {
            WorkoutExcerciseId = workoutExcerciseId,
            Name = "Bench Press",
            WorkoutId = workoutId,
            ExcerciseId = excerciseId,
            Reps = 8,
            Sets = 4,
            LoadType = Enums.LoadType.Percentage,
            Weight = null,
            Percentage = 75
        };

        var updatedWorkoutExcercise = new WorkoutExcercise(4, 8, Enums.LoadType.Percentage, percentage: 75)
        {
            Id = workoutExcerciseId,
            WorkoutId = workoutId,
            ExcerciseId = excerciseId,
            Excercise = new Excercise { Name = "Bench Press" }
        };

        _workoutExcerciseLogicMock
            .Setup(logic => logic.updateWorkoutExcercise(workoutExcerciseId, request.Name, workoutId, excerciseId,
                request.Reps, request.Sets, request.LoadType, request.Weight, request.Percentage))
            .ReturnsAsync(updatedWorkoutExcercise);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(updatedWorkoutExcercise.Id);
        result.WorkoutId.Should().Be(updatedWorkoutExcercise.WorkoutId);
        result.ExcerciseId.Should().Be(updatedWorkoutExcercise.ExcerciseId);
        result.ExcerciseName.Should().Be("Bench Press");
        result.Reps.Should().Be(updatedWorkoutExcercise.Reps);
        result.Sets.Should().Be(updatedWorkoutExcercise.Sets);
        result.LoadType.Should().Be(updatedWorkoutExcercise.LoadType);
        result.Weight.Should().Be(updatedWorkoutExcercise.Weight);
        result.Percentage.Should().Be(updatedWorkoutExcercise.Percentage);
        _workoutExcerciseLogicMock.Verify(
            logic => logic.updateWorkoutExcercise(workoutExcerciseId, request.Name, workoutId, excerciseId,
                request.Reps, request.Sets, request.LoadType, request.Weight, request.Percentage),
            Times.Once);
    }

    [TestMethod]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        var workoutExcerciseId = Guid.NewGuid();
        _workoutExcerciseLogicMock
            .Setup(logic => logic.deleteWorkoutExcercise(workoutExcerciseId))
            .Returns(Task.CompletedTask);
        var request = new DeleteWorkoutExcerciseRequestDto { WorkoutExcerciseId = workoutExcerciseId };

        // Act
        var result = await _controller.Delete(request);

        // Assert
        result.Should().BeOfType<Microsoft.AspNetCore.Mvc.NoContentResult>();
        _workoutExcerciseLogicMock.Verify(logic => logic.deleteWorkoutExcercise(workoutExcerciseId), Times.Once);
    }
}