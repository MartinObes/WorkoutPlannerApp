using FluentAssertions;
using Moq;
using WorkoutPlanner.API.Controllers;
using WorkoutPlanner.Application.Evaluations;

namespace WorkoutPlanner.API.Tests;

[TestClass]
public class EvaluationControllerTest
{
    Mock<IEvaluationLogic> _evaluationLogicMock = null!;
    EvaluationController _controller = null!;
    
    [TestInitialize]
    public void SetUp()
    {
        _evaluationLogicMock = new Mock<IEvaluationLogic>(MockBehavior.Strict);
        _controller = new EvaluationController(_evaluationLogicMock.Object);
    }
    
    [TestMethod]
    public async Task Create_ReturnsCreatedEvaluation()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var excerciseId = Guid.NewGuid();
        var reps = 10;
        var weight = 100;
        var createdEvaluation = new Domain.Evaluation { PlayerId = playerId, ExcerciseId = excerciseId, Reps = reps, Weight = weight };
        _evaluationLogicMock.Setup(logic => logic.CreateEvaluation(playerId, excerciseId, reps, weight)).ReturnsAsync(createdEvaluation);
        var request = new Models.CreateEvaluationRequestDto { PlayerId = playerId, ExcerciseId = excerciseId, Reps = reps, Weight = weight };

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.PlayerId.Should().Be(playerId);
        result.ExcerciseId.Should().Be(excerciseId);
        result.Reps.Should().Be(reps);
        result.Weight.Should().Be(weight);
        _evaluationLogicMock.Verify(logic => logic.CreateEvaluation(playerId, excerciseId, reps, weight), Times.Once);
    }
    
    [TestMethod]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        var evaluationId = Guid.NewGuid();
        _evaluationLogicMock.Setup(logic => logic.DeleteEvaluation(evaluationId)).Returns(Task.CompletedTask);
        var request = new Models.DeleteEvaluationRequestDto { EvaluationId = evaluationId };

        // Act
        var result = await _controller.Delete(request);

        // Assert
        result.Should().BeOfType<Microsoft.AspNetCore.Mvc.NoContentResult>();
        _evaluationLogicMock.Verify(logic => logic.DeleteEvaluation(evaluationId), Times.Once);
    }
    
    [TestMethod]
    public async Task GetById_ReturnsEvaluation()
    {
        // Arrange
        var evaluationId = Guid.NewGuid();
        var evaluation = new Domain.Evaluation { Id = evaluationId };
        _evaluationLogicMock.Setup(logic => logic.GetEvaluationById(evaluationId)).ReturnsAsync(evaluation);

        // Act
        var result = await _controller.GetById(evaluationId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(evaluationId);
        _evaluationLogicMock.Verify(logic => logic.GetEvaluationById(evaluationId), Times.Once);
    }
    
    [TestMethod]
    public async Task GetAllByPlayerId_ReturnsEvaluations()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var evaluations = new List<Domain.Evaluation>
        {
            new Domain.Evaluation { PlayerId = playerId },
            new Domain.Evaluation { PlayerId = playerId }
        };
        _evaluationLogicMock.Setup(logic => logic.GetEvaluationsByPlayerId(playerId)).ReturnsAsync(evaluations);
        var request = new Models.GetEvaluationsByPlayerIdRequestDto { PlayerId = playerId };

        // Act
        var result = await _controller.GetAllByPlayerId(request);

        // Assert
        result.Should().NotBeNull();
        result.Evaluations.Should().HaveCount(2);
        result.Evaluations[0].PlayerId.Should().Be(playerId);
        result.Evaluations[1].PlayerId.Should().Be(playerId);
        _evaluationLogicMock.Verify(logic => logic.GetEvaluationsByPlayerId(playerId), Times.Once);
    }
    
    [TestMethod]
    public async Task GetAllByExcerciseId_ReturnsEvaluations()
    {
        // Arrange
        var excerciseId = Guid.NewGuid();
        var evaluations = new List<Domain.Evaluation>
        {
            new Domain.Evaluation { ExcerciseId = excerciseId },
            new Domain.Evaluation { ExcerciseId = excerciseId }
        };
        _evaluationLogicMock.Setup(logic => logic.GetEvaluationsByExcerciseId(excerciseId)).ReturnsAsync(evaluations);
        var request = new Models.GetEvaluationsByExcerciseIdRequestDto { ExcerciseId = excerciseId };

        // Act
        var result = await _controller.GetAllByExcerciseId(request);

        // Assert
        result.Should().NotBeNull();
        result.Evaluations.Should().HaveCount(2);
        result.Evaluations[0].ExcerciseId.Should().Be(excerciseId);
        result.Evaluations[1].ExcerciseId.Should().Be(excerciseId);
        _evaluationLogicMock.Verify(logic => logic.GetEvaluationsByExcerciseId(excerciseId), Times.Once);
    }
    
    [TestMethod]
    public async Task CompareEvaluations_ReturnsComparisonResult()
    {
        // Arrange
        var evaluationId1 = Guid.NewGuid();
        var evaluationId2 = Guid.NewGuid();
        var comparisonResult = 1; // Assume evaluationId1 is better than evaluationId2
        _evaluationLogicMock.Setup(logic => logic.CompareEvaluations(evaluationId1, evaluationId2)).ReturnsAsync(comparisonResult);
        var request = new Models.CompareEvaluationsRequestDto { EvaluationId1 = evaluationId1, EvaluationId2 = evaluationId2 };

        // Act
        var result = await _controller.CompareEvaluations(request);

        // Assert
        result.Should().NotBeNull();
        result.Difference.Should().Be(comparisonResult);
        _evaluationLogicMock.Verify(logic => logic.CompareEvaluations(evaluationId1, evaluationId2), Times.Once);
    }
    
}