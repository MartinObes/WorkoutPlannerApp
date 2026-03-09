using FluentAssertions;
using WorkoutPlanner.Application.Evaluations;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class EvaluationLogicTest
{
    private EvaluationLogic _evaluationLogic = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _evaluationLogic = new EvaluationLogic();
    }
    
    [TestMethod]
    public void CreateEvaluation_WithValidAttributes_ShouldReturnEvaluation()
    {
        //Arrange
        Guid playerId = Guid.NewGuid();
        Guid excerciseId = Guid.NewGuid();
        int reps = 10;
        int weight = 100;
        
        //Act
        var evaluation = _evaluationLogic.CreateEvaluation(playerId, excerciseId, reps, weight);
        
        //Assert
        evaluation.Should().NotBeNull();
        evaluation.PlayerId.Should().Be(playerId);
        evaluation.ExcerciseId.Should().Be(excerciseId);
        evaluation.Reps.Should().Be(reps);
        evaluation.Weight.Should().Be(weight);
    }
    
    [TestMethod]
    public void CreateEvaluation_WithZeroReps_ShouldThrowArgumentException()
    {
        //Arrange
        Guid playerId = Guid.NewGuid();
        Guid excerciseId = Guid.NewGuid();
        int reps = 0;
        int weight = 100;
        
        //Act
        Action act = () => _evaluationLogic.CreateEvaluation(playerId, excerciseId, reps, weight);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Reps must be greater than 0");
    }
    
    [TestMethod]
    public void CreateEvaluation_WithNegativeWeight_ShouldThrowArgumentException()
    {
        //Arrange
        Guid playerId = Guid.NewGuid();
        Guid excerciseId = Guid.NewGuid();
        int reps = 10;
        int weight = -1;
        
        //Act
        Action act = () => _evaluationLogic.CreateEvaluation(playerId, excerciseId, reps, weight);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Weight must be greater than or equal to 0");
    }

    
}