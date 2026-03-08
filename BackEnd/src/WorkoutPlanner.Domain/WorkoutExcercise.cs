using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Domain;

public class WorkoutExcercise
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    public Guid ExcerciseId { get; set; }
    public Excercise Excercise { get; set; } = null!;
    public int sets { get; set; }
    public int Reps { get; set; }
    public LoadType LoadType { get; set; }
    public int? Weight { get; set; }
    public int? Percentage { get; set; }
}