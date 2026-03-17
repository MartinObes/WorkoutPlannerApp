using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using WorkoutPlanner.Application.Evaluations;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Repositories;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class EvaluationLogicTest
{
    private EvaluationLogic _evaluationLogic = null!;
    private Mock<EvaluationRepository> _evaluationRepositoryMock = null!;
    private readonly DbContextOptions<AppDbContext> _dummyOptions = new();

    [TestInitialize]
    public void Setup()
    {
        var mockDbContext = new Mock<AppDbContext>(_dummyOptions);
        _evaluationRepositoryMock = new Mock<EvaluationRepository>(MockBehavior.Strict, mockDbContext.Object);
        _evaluationLogic = new EvaluationLogic(_evaluationRepositoryMock.Object);
    }

    [TestMethod]
    public async Task CreateEvaluation_WithValidAttributes_ShouldReturnEvaluation()
    {
        // Arrange
        Guid playerId = Guid.NewGuid();
        Guid excerciseId = Guid.NewGuid();
        int reps = 10;
        int weight = 100;
        _evaluationRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Evaluation>()))
            .Returns(Task.CompletedTask);

        // Act
        var evaluation = await _evaluationLogic.CreateEvaluation(playerId, excerciseId, reps, weight);

        // Assert
        evaluation.Should().NotBeNull();
        evaluation.PlayerId.Should().Be(playerId);
        evaluation.ExcerciseId.Should().Be(excerciseId);
        evaluation.Reps.Should().Be(reps);
        evaluation.Weight.Should().Be(weight);
        _evaluationRepositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<Evaluation>(e =>
                e.PlayerId == playerId &&
                e.ExcerciseId == excerciseId &&
                e.Reps == reps &&
                e.Weight == weight)), Times.Once);
    }

    [TestMethod]
    public void CreateEvaluation_WithZeroReps_ShouldThrowArgumentException()
    {
        // Arrange
        Guid playerId = Guid.NewGuid();
        Guid excerciseId = Guid.NewGuid();
        int reps = 0;
        int weight = 100;
        _evaluationRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Evaluation>()))
            .Returns(Task.CompletedTask);

        // Act
        Action act = () => _evaluationLogic.CreateEvaluation(playerId, excerciseId, reps, weight).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Reps must be greater than 0");
        _evaluationRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Evaluation>()), Times.Never);
    }

    [TestMethod]
    public void CreateEvaluation_WithNegativeWeight_ShouldThrowArgumentException()
    {
        // Arrange
        Guid playerId = Guid.NewGuid();
        Guid excerciseId = Guid.NewGuid();
        int reps = 10;
        int weight = -1;
        _evaluationRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<Evaluation>()))
            .Returns(Task.CompletedTask);

        // Act
        Action act = () => _evaluationLogic.CreateEvaluation(playerId, excerciseId, reps, weight).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Weight must be greater than or equal to 0");
        _evaluationRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Evaluation>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteEvaluation_WhenValidEvaluation_CallsRepositoryDelete()
    {
        // Arrange
        var evaluation = new Evaluation
        {
            Id = Guid.NewGuid(),
            PlayerId = Guid.NewGuid(),
            ExcerciseId = Guid.NewGuid(),
            Reps = 6,
            Weight = 75,
            Date = DateTime.UtcNow
        };

        _evaluationRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(evaluation);
        _evaluationRepositoryMock
            .Setup(repo => repo.Delete(evaluation))
            .Verifiable();

        // Act
        await _evaluationLogic.DeleteEvaluation(evaluation.Id);

        // Assert
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
        _evaluationRepositoryMock.Verify(repo => repo.Delete(evaluation), Times.Once);
    }

    [TestMethod]
    public void DeleteEvaluation_WhenEvaluationDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var evaluationId = Guid.NewGuid();
        _evaluationRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync((Evaluation)null!);

        // Act
        Action act = () => _evaluationLogic.DeleteEvaluation(evaluationId).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage($"Evaluation with id {evaluationId} not found.");
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
        _evaluationRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Evaluation>()), Times.Never);
    }

    [TestMethod]
    public async Task GetEvaluationById_WhenEvaluationExists_ReturnsEvaluation()
    {
        // Arrange
        Guid evaluationId = Guid.NewGuid();
        var evaluation = new Evaluation
        {
            Id = evaluationId,
            PlayerId = Guid.NewGuid(),
            ExcerciseId = Guid.NewGuid(),
            Reps = 9,
            Weight = 85,
            Date = DateTime.UtcNow
        };

        _evaluationRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(evaluation);

        // Act
        var result = await _evaluationLogic.GetEvaluationById(evaluationId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(evaluationId);
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public void GetEvaluationById_WhenEvaluationDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        Guid evaluationId = Guid.NewGuid();
        _evaluationRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync((Evaluation)null!);

        // Act
        Action act = () => _evaluationLogic.GetEvaluationById(evaluationId).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage($"Evaluation with id {evaluationId} not found.");
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task GetEvaluationsByPlayerId_WhenEvaluationsExist_ReturnsList()
    {
        // Arrange
        Guid playerId = Guid.NewGuid();
        var evaluations = new List<Evaluation>
        {
            new() { Id = Guid.NewGuid(), PlayerId = playerId, ExcerciseId = Guid.NewGuid(), Reps = 10, Weight = 100, Date = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), PlayerId = playerId, ExcerciseId = Guid.NewGuid(), Reps = 12, Weight = 105, Date = DateTime.UtcNow }
        };

        _evaluationRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(evaluations);

        // Act
        var result = await _evaluationLogic.GetEvaluationsByPlayerId(playerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(e => e.PlayerId == playerId);
        _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task GetEvaluationsByPlayerId_WhenNoEvaluationsExist_ReturnsEmptyList()
    {
        // Arrange
        Guid playerId = Guid.NewGuid();
        _evaluationRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(new List<Evaluation>());

        // Act
        var result = await _evaluationLogic.GetEvaluationsByPlayerId(playerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task GetEvaluationsByExcerciseId_WhenEvaluationsExist_ReturnsList()
    {
        // Arrange
        Guid excerciseId = Guid.NewGuid();
        var evaluations = new List<Evaluation>
        {
            new() { Id = Guid.NewGuid(), PlayerId = Guid.NewGuid(), ExcerciseId = excerciseId, Reps = 10, Weight = 80, Date = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), PlayerId = Guid.NewGuid(), ExcerciseId = excerciseId, Reps = 11, Weight = 82, Date = DateTime.UtcNow }
        };

        _evaluationRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(evaluations);

        // Act
        var result = await _evaluationLogic.GetEvaluationsByExcerciseId(excerciseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(e => e.ExcerciseId == excerciseId);
        _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task GetEvaluationsByExcerciseId_WhenNoEvaluationsExist_ReturnsEmptyList()
    {
        // Arrange
        Guid excerciseId = Guid.NewGuid();
        _evaluationRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(new List<Evaluation>());

        // Act
        var result = await _evaluationLogic.GetEvaluationsByExcerciseId(excerciseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public async Task CompareEvaluations_WhenFirstIsHeavier_ReturnsPositiveDifference()
    {
        // Arrange
        Guid evalId1 = Guid.NewGuid();
        Guid evalId2 = Guid.NewGuid();
        var eval1 = new Evaluation { Id = evalId1, Weight = 120, Reps = 8, PlayerId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid(), Date = DateTime.UtcNow };
        var eval2 = new Evaluation { Id = evalId2, Weight = 90, Reps = 8, PlayerId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid(), Date = DateTime.UtcNow };

        _evaluationRepositoryMock
            .SetupSequence(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(eval1)
            .ReturnsAsync(eval2);

        // Act
        var result = await _evaluationLogic.CompareEvaluations(evalId1, evalId2);

        // Assert
        result.Should().Be(30);
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Exactly(2));
    }

    [TestMethod]
    public async Task CompareEvaluations_WhenSecondIsHeavier_ReturnsNegativeDifference()
    {
        // Arrange
        Guid evalId1 = Guid.NewGuid();
        Guid evalId2 = Guid.NewGuid();
        var eval1 = new Evaluation { Id = evalId1, Weight = 75, Reps = 10, PlayerId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid(), Date = DateTime.UtcNow };
        var eval2 = new Evaluation { Id = evalId2, Weight = 95, Reps = 10, PlayerId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid(), Date = DateTime.UtcNow };

        _evaluationRepositoryMock
            .SetupSequence(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(eval1)
            .ReturnsAsync(eval2);

        // Act
        var result = await _evaluationLogic.CompareEvaluations(evalId1, evalId2);

        // Assert
        result.Should().Be(-20);
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Exactly(2));
    }

    [TestMethod]
    public void CompareEvaluations_WhenFirstEvaluationDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        Guid evalId1 = Guid.NewGuid();
        Guid evalId2 = Guid.NewGuid();

        _evaluationRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync((Evaluation)null!);

        // Act
        Action act = () => _evaluationLogic.CompareEvaluations(evalId1, evalId2).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage($"Evaluation with id {evalId1} not found.");
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public void CompareEvaluations_WhenSecondEvaluationDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        Guid evalId1 = Guid.NewGuid();
        Guid evalId2 = Guid.NewGuid();
        var eval1 = new Evaluation { Id = evalId1, Weight = 80, Reps = 8, PlayerId = Guid.NewGuid(), ExcerciseId = Guid.NewGuid(), Date = DateTime.UtcNow };

        _evaluationRepositoryMock
            .SetupSequence(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null))
            .ReturnsAsync(eval1)
            .ReturnsAsync((Evaluation)null!);

        // Act
        Action act = () => _evaluationLogic.CompareEvaluations(evalId1, evalId2).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage($"Evaluation with id {evalId2} not found.");
        _evaluationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Evaluation, bool>>>(), null), Times.Exactly(2));
    }


}