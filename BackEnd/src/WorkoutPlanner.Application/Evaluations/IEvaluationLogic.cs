using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Evaluations;

public interface IEvaluationLogic
{
    public Evaluation CreateEvaluation(Guid playerId, Guid excerciseId, int reps, int weight);
}