using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.API.Models;

public class ProcessWorkoutRequestDto
{
    public string WorkoutName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}

public class ProcessedExerciseResponseDto
{
    public string ExerciseName { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int? CalculatedWeight { get; set; }
    public Enums.LoadType LoadType { get; set; }
    public int? OriginalPercentage { get; set; }

    public ProcessedExerciseResponseDto()
    {
    }

    public ProcessedExerciseResponseDto(ProcessedExercise processedExercise)
    {
        ExerciseName = processedExercise.ExerciseName;
        Sets = processedExercise.Sets;
        Reps = processedExercise.Reps;
        CalculatedWeight = processedExercise.CalculatedWeight;
        LoadType = processedExercise.LoadType;
        OriginalPercentage = processedExercise.OriginalPercentage;
    }
}

public class ProcessedWorkoutResponseDto
{
    public IList<ProcessedExerciseResponseDto> Exercises { get; set; } = new List<ProcessedExerciseResponseDto>();

    public ProcessedWorkoutResponseDto()
    {
    }

    public ProcessedWorkoutResponseDto(IList<ProcessedExercise> exercises)
    {
        Exercises = exercises.Select(exercise => new ProcessedExerciseResponseDto(exercise)).ToList();
    }
}
