namespace WorkoutPlanner.Domain.AuxiliaryDomainClasses;

public class CreateWorkoutExcerciseArgs
{
    public Guid WorkoutId { get; set; }
    public Guid ExcerciseId { get; set; }
    public int Sets { get;  set; }
    public int Reps { get;  set; }
    public Enums.LoadType LoadType { get;  set; }
    public int? Weight { get;  set; }
    public int? Percentage { get;  set; }
}