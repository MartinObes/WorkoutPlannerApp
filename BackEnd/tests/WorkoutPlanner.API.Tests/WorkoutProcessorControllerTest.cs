using FluentAssertions;
using Moq;
using WorkoutPlanner.API.Controllers;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Services.WourkoutProcessorService;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.API.Tests;

[TestClass]
public class WorkoutProcessorControllerTest
{
    private Mock<IWorkoutProcessorService> _workoutProcessorServiceMock = null!;
    private ProcessWorkoutController _controller = null!;

    [TestInitialize]
    public void SetUp()
    {
        _workoutProcessorServiceMock = new Mock<IWorkoutProcessorService>(MockBehavior.Strict);
        _controller = new ProcessWorkoutController(_workoutProcessorServiceMock.Object);
    }
    
    [TestMethod]
    public async Task ProcessWorkout_ValidRequest_ReturnsProcessedWorkoutResponseDto()
    {
        //Arrange
        var workoutName = "Workout1";
        var username = "User1";
        var request = new ProcessWorkoutRequestDto
        {
            WorkoutName = workoutName,
            Username = username
        };

        var processedWorkoutResult = new List<ProcessedExercise>
        {
            new ProcessedExercise
            {
                ExerciseName = "Exercise1",
                Sets = 3,
                Reps = 10,
                CalculatedWeight = 100,
                LoadType = Domain.Enums.LoadType.Weight,
                OriginalPercentage = null
            },
            new ProcessedExercise
            {
                ExerciseName = "Exercise2",
                Sets = 4,
                Reps = 8,
                CalculatedWeight = null,
                LoadType = Domain.Enums.LoadType.Percentage,
                OriginalPercentage = 80
            }
        };
        var expectedResponse = new ProcessedWorkoutResponseDto(processedWorkoutResult);

        _workoutProcessorServiceMock
            .Setup(service => service.ProcessWorkout(workoutName, username))
            .ReturnsAsync(processedWorkoutResult);

        //Act
        var actionResult = await _controller.ProcessWorkout(request);
        
        //Assert
        var response = actionResult.Should().BeOfType<ProcessedWorkoutResponseDto>().Subject;
        response.Should().BeEquivalentTo(expectedResponse);
        
        _workoutProcessorServiceMock.Verify(service => service.ProcessWorkout(workoutName, username), Times.Once);
    }
    
}