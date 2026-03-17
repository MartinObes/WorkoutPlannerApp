using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Models;

public class CreateEvaluationRequestDto
{
    public Guid PlayerId { get; set; }
    public Guid ExcerciseId { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; }
}

public class CompareEvaluationsResponseDto
{
    public int Difference { get; set; }

    public CompareEvaluationsResponseDto()
    {
    }

    public CompareEvaluationsResponseDto(int difference)
    {
        Difference = difference;
    }
}

public class EvaluationResponseDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid ExcerciseId { get; set; }
    public DateTime Date { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; }

    public EvaluationResponseDto()
    {
    }

    public EvaluationResponseDto(Evaluation evaluation)
    {
        Id = evaluation.Id;
        PlayerId = evaluation.PlayerId;
        ExcerciseId = evaluation.ExcerciseId;
        Date = evaluation.Date;
        Reps = evaluation.Reps;
        Weight = evaluation.Weight;
    }
}

public class EvaluationsResponseDto
{
    public IList<EvaluationResponseDto> Evaluations { get; set; } = new List<EvaluationResponseDto>();

    public EvaluationsResponseDto()
    {
    }

    public EvaluationsResponseDto(IList<Evaluation> evaluations)
    {
        Evaluations = evaluations.Select(evaluation => new EvaluationResponseDto(evaluation)).ToList();
    }
}
