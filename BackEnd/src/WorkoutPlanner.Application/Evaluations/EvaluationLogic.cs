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
        return eval;
    }

    public void DeleteEvaluation(Evaluation evaluation)
    {
        if(evaluation == null)
        {
            throw new ArgumentException("Evaluation cannot be null.");
        }
        
        _evaluationRepository.Delete(evaluation);
    }

    public async Task<Evaluation> GetEvaluationById(Guid id)
    {
        var evaluation = await _evaluationRepository.GetAsync(e => e.Id == id);
        if(evaluation == null)
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
}