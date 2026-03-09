using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Domain;

public class WorkoutExcercise
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    public Guid ExcerciseId { get; set; }
    public Excercise Excercise { get; set; } = null!;
    public int Sets { get; private set; }
    public int Reps { get; private set; }
    public LoadType LoadType { get; private set; }
    public int? Weight { get; private set; }
    public int? Percentage { get; private set; }
    
    public WorkoutExcercise(int sets, int reps, LoadType loadType, int? weight = null, int? percentage = null)
    {
        Validate(sets, reps, loadType, weight, percentage);
        Sets = sets;
        Reps = reps;
        LoadType = loadType;
        Weight = weight;
        Percentage = percentage;
    }
    
    public static void Validate(int sets, int reps, LoadType loadType, int? weight, int? percentage)
    {
        if (sets <= 0)
        {
            throw new ArgumentException("Sets must be greater than 0");
        }

        if (reps <= 0)
        {
            throw new ArgumentException("Reps must be greater than 0");
        }

        if (loadType == LoadType.Weight && weight == null)
        {
            throw new ArgumentException("Weight must be provided");
        }

        if (loadType == LoadType.Percentage && percentage == null)
        {
            throw new ArgumentException("Percentage must be provided");
        }
        
        if(!Enum.IsDefined(typeof(LoadType), loadType))
        {
            throw new ArgumentException("Invalid load type.");
        }
    }
}