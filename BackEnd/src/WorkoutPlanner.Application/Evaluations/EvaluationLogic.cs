using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Evaluations;

public class EvaluationLogic(IEvaluationRepository evaluationRepository) : IEvaluationLogic
{
    private readonly IEvaluationRepository _evaluationRepository = evaluationRepository ?? throw new ArgumentNullException(nameof(evaluationRepository));
    public async Task<Evaluation> CreateEvaluation(Guid playerId, Guid excerciseId, int reps, int weight)
    {
        Evaluation.Validate(reps, weight);
        var eval = new Evaluation
        {
            PlayerId = playerId,
            ExcerciseId = excerciseId,
            Reps = reps,
            Weight = weight,
            Date = DateTime.UtcNow
        };

        await _evaluationRepository.InsertAsync(eval);
        await _evaluationRepository.SaveAsync();
        return eval;
    }

    public async Task DeleteEvaluation(Guid evaluationId)
    {
        var evaluation = await GetEvaluationById(evaluationId);
        if (evaluation == null)
        {
            throw new ArgumentException("Evaluation cannot be null.");
        }

        _evaluationRepository.Delete(evaluation);
        await _evaluationRepository.SaveAsync();
    }

    public async Task<Evaluation> GetEvaluationById(Guid id)
    {
        var evaluation = await _evaluationRepository.GetAsync(e => e.Id == id);
        if (evaluation == null)
        {
            throw new KeyNotFoundException($"Evaluation with id {id} not found.");
        }

        return evaluation;
    }

    public async Task<IList<Evaluation>> GetEvaluationsByPlayerId(Guid playerId)
    {
        var evaluations = await _evaluationRepository.GetAllAsync(e => e.PlayerId == playerId);
        return evaluations;
    }

    public async Task<IList<Evaluation>> GetEvaluationsByExcerciseId(Guid excerciseId)
    {
        var evaluations = await _evaluationRepository.GetAllAsync(e => e.ExcerciseId == excerciseId);
        return evaluations;
    }

    public async Task<int> CompareEvaluations(Guid evalid1, Guid evalid2)
    {
        var eval1 = await GetEvaluationById(evalid1);
        var eval2 = await GetEvaluationById(evalid2);

        var result = eval1.Weight - eval2.Weight;
        return result;
    }
}