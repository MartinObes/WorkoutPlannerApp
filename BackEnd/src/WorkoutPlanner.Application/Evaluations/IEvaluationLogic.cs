using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Evaluations;

public interface IEvaluationLogic
{
    public Task<Evaluation> CreateEvaluation(Guid playerId, Guid excerciseId, int reps, int weight);
    public Task DeleteEvaluation(Guid evaluationId);
    public Task<Evaluation> GetEvaluationById(Guid id);
    public Task<IList<Evaluation>> GetEvaluationsByPlayerId(Guid playerId);
    public Task<IList<Evaluation>> GetEvaluationsByExcerciseId(Guid excerciseId);
    public Task<int> CompareEvaluations(Guid evaid1, Guid evalid2);
}