using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Evaluations;

public class EvaluationLogic : IEvaluationLogic
{
    public Evaluation CreateEvaluation(Guid playerId, Guid excerciseId, int reps, int weight)
    {
        Evaluation.Validate(reps, weight);
        return new Evaluation
        {
            PlayerId = playerId,
            ExcerciseId = excerciseId,
            Reps = reps,
            Weight = weight,
            Date = DateTime.UtcNow
        };
    }

}