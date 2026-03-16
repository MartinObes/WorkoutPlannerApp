using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Domain.AuxiliaryDomainClasses;

public class ProcessedExercise
{
    public string ExerciseName { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int? CalculatedWeight { get; set; }   // null = no evaluation found, display OriginalPercentage instead
    public LoadType LoadType { get; set; }
    public int? OriginalPercentage { get; set; }
}

